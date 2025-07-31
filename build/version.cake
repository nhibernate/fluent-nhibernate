public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string VersionSuffix { get; private set; }
    public string Milestone { get; private set; }
    public string AppVersion { get; private set; }
    public string AssemblyVersion { get; private set; }
    public string InformationalVersion {get; private set; }

    public static BuildVersion Calculate(ICakeContext context, BuildParameters parameters)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = null;
        string semVersion = null;
        string assemblyVersion = null;
        string informationalVersion = null;
        string milestone = null;
        string preReleaseTag = null;

        context.Information("Calculating Semantic Version");
        if (!parameters.IsLocalBuild || parameters.IsPublishBuild || parameters.IsReleaseBuild)
        {
            context.GitVersion(new GitVersionSettings{
                UpdateAssemblyInfo = false,
                OutputType = GitVersionOutput.BuildServer
            });

            version = context.EnvironmentVariable("GitVersion_MajorMinorPatch");
            semVersion = context.EnvironmentVariable("GitVersion_SemVer");
            preReleaseTag = context.EnvironmentVariable("GitVersion_PreReleaseTag");
            assemblyVersion = context.EnvironmentVariable("GitVersion_AssemblySemVer");
            informationalVersion = context.EnvironmentVariable("GitVersion_InformationalVersion");
            milestone = string.Concat("v", version);
        }

        GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        });

        version = assertedVersions.MajorMinorPatch;
        semVersion = assertedVersions.SemVer;
        preReleaseTag = assertedVersions.PreReleaseTag;
        assemblyVersion = assertedVersions.AssemblySemVer;
        informationalVersion = assertedVersions.InformationalVersion;
        milestone = string.Concat("v", version);

        context.Information("Calculated Semantic Version: {0}", semVersion);

        var appVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion,
            VersionSuffix = preReleaseTag,
            Milestone = milestone,
            AppVersion = appVersion,
            AssemblyVersion = assemblyVersion,
            InformationalVersion = informationalVersion,
        };
    }
}