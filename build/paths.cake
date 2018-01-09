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
        var testResultsDir = artifactsDir.Combine("test-results");
        var nuspecRoot = (DirectoryPath)"nuspec";
        var nugetRoot = artifactsDir.Combine("nuget");
        
        var zipArtifactPathDesktop = artifactsDir.CombineWithFilePath($"FluentHibernate-net461-v{semVersion}.zip");        

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nuspecRoot,
            nugetRoot,
            artifactsBinDir,
            artifactsBinFullFx);

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
    public DirectoryPath NuspecRoot { get; private set; }
    public DirectoryPath NugetRoot { get; private set; }
    public DirectoryPath ArtifactsBin { get; private set; }
    public DirectoryPath ArtifactsBinFullFx { get; private set; }    
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(        
        DirectoryPath artifactsDir,
        DirectoryPath testResultsDir,
        DirectoryPath nuspecRoot,
        DirectoryPath nugetRoot,
        DirectoryPath artifactsBinDir,
        DirectoryPath artifactsBinFullFx)
    {
        Artifacts = artifactsDir;
        TestResults = testResultsDir;
        NuspecRoot = nuspecRoot;
        NugetRoot = nugetRoot;
        ArtifactsBin = artifactsBinDir;
        ArtifactsBinFullFx = artifactsBinFullFx;        
        ToClean = new[] {
            Artifacts,
            TestResults,
            NugetRoot,
            ArtifactsBin,
            ArtifactsBinFullFx            
        };
    }
}