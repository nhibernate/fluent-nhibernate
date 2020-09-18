public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }

    public static BuildPaths GetPaths(
        ICakeContext context,
        string configuration,
        string semVersion)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        if (string.IsNullOrEmpty(configuration))
        {
            throw new ArgumentNullException("configuration");
        }
        if (string.IsNullOrEmpty(semVersion))
        {
            throw new ArgumentNullException("semVersion");
        }
        
        var artifactsDir = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var artifactsBinDir = artifactsDir.Combine("bin");
        var artifactsBinFullFx = artifactsBinDir.Combine("net461");        
        var artifactsBinNetStandard20 = artifactsBinDir.Combine("netstandard2.0");        
        var artifactsBinNetCoreapp2 = artifactsBinDir.Combine("netcoreapp2.0");        
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");
        
        var zipArtifactPathDesktop = artifactsDir.CombineWithFilePath($"FluentHibernate-net461-v{semVersion}.zip");        

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            artifactsBinDir,
            artifactsBinFullFx,
            artifactsBinNetStandard20,
            artifactsBinNetCoreapp2);

        // Files
        var buildFiles = new BuildFiles(
            context,            
            zipArtifactPathDesktop);

        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories
        };
    }
}

public class BuildFiles
{        
    public FilePath ZipArtifactPathDesktop { get; private set; }    

    public BuildFiles(
        ICakeContext context,
        FilePath zipArtifactPathDesktop)
    {             
        ZipArtifactPathDesktop = zipArtifactPathDesktop;     
    }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; private set; }
    public DirectoryPath TestResults { get; private set; }
    public DirectoryPath NugetRoot { get; private set; }
    public DirectoryPath ArtifactsBin { get; private set; }
    public DirectoryPath ArtifactsBinFullFx { get; private set; }    
    public DirectoryPath ArtifactsBinNetStandard20 { get; private set; }    
    public DirectoryPath ArtifactsBinNetCoreApp2 { get; private set; }    
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(        
        DirectoryPath artifactsDir,
        DirectoryPath testResultsDir,
        DirectoryPath nugetRoot,
        DirectoryPath artifactsBinDir,
        DirectoryPath artifactsBinFullFx,
        DirectoryPath artifactsBinNetStandard20,
        DirectoryPath artifactsBinNetCoreapp2
        )
    {
        Artifacts = artifactsDir;
        TestResults = testResultsDir;
        NugetRoot = nugetRoot;
        ArtifactsBin = artifactsBinDir;
        ArtifactsBinFullFx = artifactsBinFullFx;        
        ArtifactsBinNetStandard20 = artifactsBinNetStandard20;
        ArtifactsBinNetCoreApp2 = artifactsBinNetCoreapp2;
        ToClean = new[] {
            Artifacts,
            TestResults,
            NugetRoot,
            ArtifactsBin,
            ArtifactsBinFullFx            
        };
    }
}