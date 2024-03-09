#addin "nuget:?package=Cake.FileHelpers&version=7.0.0"
#tool "dotnet:?package=GitReleaseManager.Tool&version=0.17.0"
#tool "dotnet:?package=GitVersion.Tool&version=5.12.0"

#load "./build/parameters.cake"

BuildParameters parameters = BuildParameters.GetParameters(Context);
DotNetMSBuildSettings msBuildSettings = null;
bool publishingError = false;

var SolutionPath = "./src/FluentNHibernate.sln";
var SrcProjects = new [] { "FluentNHibernate" };
var TestProjects = new [] { "FluentNHibernate.Testing" };
var SpecProjects = new [] { "FluentNHibernate.Specs" };

Setup((context) =>
{    
    parameters.Initialize(context);

    Information("FluentNHibernate");
    Information($"SemVersion: {parameters.Version.SemVersion}");
    Information($"AssemblyVersion: {parameters.Version.AssemblyVersion}");
    Information($"Version: {parameters.Version.Version}");
    Information($"IsLocalBuild: {parameters.IsLocalBuild}");    
    Information($"IsTagged: {parameters.IsTagged}");
    Information($"IsPullRequest: {parameters.IsPullRequest}");
    Information($"Target: {parameters.Target}");   

    var releaseNotes = string.Join("\n", 
        parameters.ReleaseNotes.Notes.ToArray()).Replace("\"", "\"\"");

    msBuildSettings = new DotNetMSBuildSettings()
        .WithProperty("Version", parameters.Version.SemVersion)
        .WithProperty("AssemblyVersion", parameters.Version.AssemblyVersion)
        .WithProperty("FileVersion", parameters.Version.Version)
        .WithProperty("InformationalVersion", parameters.Version.InformationalVersion)
        .WithProperty("PackageReleaseNotes", string.Concat("\"", releaseNotes, "\""));
});

Teardown((context) =>
{
});

Task("Clean")
  .Does(() =>
    {
        CleanDirectories(parameters.Paths.Directories.ToClean);
        DotNetClean(SolutionPath);
        EnsureDirectoryExists(parameters.Paths.Directories.Artifacts);
        EnsureDirectoryExists(parameters.Paths.Directories.ArtifactsBinFullFx);
        EnsureDirectoryExists(parameters.Paths.Directories.TestResults);
        EnsureDirectoryExists(parameters.Paths.Directories.NugetRoot);
  });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetRestore(SolutionPath, new DotNetRestoreSettings
        {
            Verbosity = DotNetVerbosity.Minimal,            
        });
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetBuild(SolutionPath, new DotNetBuildSettings
        {
            Configuration = parameters.Configuration,
            MSBuildSettings = msBuildSettings
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {       
        var unitProjects = GetFiles("./src/**/*.Testing.csproj");
        var specProjects = GetFiles("./src/**/*.Specs.csproj");
        var testProjects = unitProjects.Union(specProjects).ToArray();

        foreach(var project in testProjects) 
        {                      
            DotNetTest(project.ToString(), new DotNetTestSettings
            {
                Framework = "net48",
                NoBuild = true,
                NoRestore = true,
                Configuration = parameters.Configuration
            });          

            DotNetTest(project.ToString(), new DotNetTestSettings
            {
                Framework = "net6.0",
                NoBuild = true,
                NoRestore = true,
                Configuration = parameters.Configuration
            });          
        }
    });

Task("Copy-Files")
    .IsDependentOn("Test")
    .Does(() =>
    {            
        PublishProjects(
            SrcProjects, "net461",
            parameters.Paths.Directories.ArtifactsBinFullFx.FullPath, 
            parameters.Version.DotNetAsterix, 
            parameters.Configuration, 
            msBuildSettings
        );
        PublishProjects(
            SrcProjects, "netstandard2.0",
            parameters.Paths.Directories.ArtifactsBinNetStandard20.FullPath, 
            parameters.Version.DotNetAsterix, 
            parameters.Configuration, 
            msBuildSettings
        );
        PublishProjects(
            SrcProjects, "netcoreapp2.0",
            parameters.Paths.Directories.ArtifactsBinNetCoreApp2.FullPath, 
            parameters.Version.DotNetAsterix, 
            parameters.Configuration, 
            msBuildSettings
        );
        
        CopyFileToDirectory("./LICENSE", parameters.Paths.Directories.ArtifactsBinFullFx);            
    });

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {            
        Zip(parameters.Paths.Directories.ArtifactsBinFullFx, parameters.Paths.Files.ZipArtifactPathDesktop, 
            GetFiles($"{parameters.Paths.Directories.ArtifactsBinFullFx.FullPath}/**/*"));
    });
  
Task("Create-NuGet-Packages")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {    
        PackProjects(
            SrcProjects, 
            parameters.Configuration,
            parameters.Paths.Directories.NugetRoot.FullPath);
    });

Task("Publish-Nuget")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => parameters.ShouldPublish)
    .Does(() =>
    {        
        foreach(var project in SrcProjects)
        {            
            var packagePath = parameters.Paths.Directories.NugetRoot
                .CombineWithFilePath(string.Concat(project, ".", parameters.Version.SemVersion, ".nupkg"));
            NuGetPush(packagePath, new NuGetPushSettings {
                Source = parameters.NuGet.ApiUrl,
                ApiKey = parameters.NuGet.ApiKey
            });
        }
   });    

Task("Generate-Docs")   
    .IsDependentOn("Build")
    .Does(() =>
    {
        // TODO  build/docu/docu.exe...  and publish to gh-pages     
    });

Task("Publish-GitHub-Release")
    .WithCriteria(() => parameters.ShouldPublish)
    .Does(() =>
    {
        GitReleaseManagerAddAssets(
            parameters.GitHub.Token, parameters.GitHub.Owner, parameters.GitHub.Repository, 
            parameters.Version.Milestone, 
            parameters.Paths.Files.ZipArtifactPathDesktop.ToString());
        GitReleaseManagerClose(
            parameters.GitHub.Token, parameters.GitHub.Owner, parameters.GitHub.Repository, 
            parameters.Version.Milestone);
    })
    .OnError(exception =>
    {
        Information("Publish-GitHub-Release Task failed, but continuing with next Task...");
        publishingError = true;
    });

Task("Create-Release-Notes")
    .Does(() =>
    {
        GitReleaseManagerCreate(
            parameters.GitHub.Token, 
            parameters.GitHub.Owner, parameters.GitHub.Repository, 
            new GitReleaseManagerCreateSettings {
                Milestone         = parameters.Version.Milestone,
                Name              = parameters.Version.Milestone,
                Prerelease        = true,
                TargetCommitish   = "main"
            }
        );
    });

Task("Update-AppVeyor-BuildNumber")
    .WithCriteria(() => parameters.IsRunningOnAppVeyor)
    .Does(() =>
    {
        // AppVeyor.UpdateBuildVersion(parameters.Version.SemVersion);
    })
    .ReportError(exception =>
    {
        // Via: See https://github.com/reactiveui/ReactiveUI/issues/1262
        Warning("Build with version {0} already exists.", parameters.Version.SemVersion);
    });    

Task("Upload-AppVeyor-Artifacts")        
    .WithCriteria(() => parameters.IsRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPathDesktop);    
        foreach(var package in GetFiles(parameters.Paths.Directories.NugetRoot + "/*"))
        {
            AppVeyor.UploadArtifact(package);
        }
    });
	
Task("Release-Notes")
  .IsDependentOn("Create-Release-Notes");

Task("Package")
    .IsDependentOn("Zip-Files")
    .IsDependentOn("Create-NuGet-Packages");  

Task("AppVeyor")
    .IsDependentOn("Update-AppVeyor-BuildNumber")
    .IsDependentOn("Package")
    .IsDependentOn("Upload-AppVeyor-Artifacts")     
    .IsDependentOn("Publish-NuGet")
    .IsDependentOn("Publish-GitHub-Release")
    .Finally(() =>
    {
        if(publishingError)
        {
            throw new Exception("An error occurred during the publishing of Cake.  All publishing tasks have been attempted.");
        }
    });
    
Task("Default")
    .IsDependentOn("Package");
    
RunTarget(parameters.Target);

private void PublishProjects(
    IEnumerable<string> projectNames,
    string framework,
    string artifactsBin,
    string versionSuffix,
    string configuration, 
    DotNetMSBuildSettings msBuildSettings)
{
    foreach(var project in projectNames)
    {        
        DotNetPublish($"./src/{project}", new DotNetPublishSettings
        {
            Framework = framework,
            VersionSuffix = versionSuffix,
            Configuration = configuration,
            OutputDirectory = artifactsBin,
            MSBuildSettings = msBuildSettings
        });
     
        // Copy documentation XML (since publish does not do this anymore)
        CopyFileToDirectory($"./src/{project}/bin/{configuration}/{framework}/{project}.xml", artifactsBin);    
    }
}

private void PackProjects(
    IEnumerable<string> projectNames, 
    string configuration,
    string nugetDir)
{
    foreach(var project in projectNames) {
        var projectPath = File($"./src/{project}/{project}.csproj");
        DotNetPack(projectPath.ToString(), new DotNetPackSettings
        {
            Configuration = configuration,
            MSBuildSettings = msBuildSettings,
            OutputDirectory = nugetDir,
            IncludeSymbols = true
        });
    }
}
