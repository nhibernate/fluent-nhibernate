#load "./paths.cake"
#load "./version.cake"
#load "./credentials.cake"

public class BuildParameters
{
    public string Target { get; private set; }
    public string Configuration { get; private set; }
    public bool IsLocalBuild { get; private set; }    
    public bool IsRunningOnWindows { get; private set; }
    public bool IsCI { get; private set; }
    public bool IsPullRequest { get; private set; }
    public bool IsMainRepo { get; private set; }
    public bool IsMainBranch { get; private set; }    
    public bool IsTagged { get; private set; }
    public bool IsPublishBuild { get; private set; }
    public bool IsReleaseBuild { get; private set; }    
    public ReleaseNotes ReleaseNotes { get; private set; }
    public BuildGitHub GitHub { get; private set; }            
    public BuildNuGet NuGet { get; private set; }            
    public BuildVersion Version { get; private set; }
    public BuildPaths Paths { get; private set; }  

    public bool ShouldPublish =>
        !IsLocalBuild && 
        !IsPullRequest && 
        IsMainRepo &&
        IsMainBranch && 
        IsTagged;            
    
    public void Initialize(ICakeContext context)
    {
        Version = BuildVersion.Calculate(context, this);

        Paths = BuildPaths.GetPaths(context, Configuration, Version.SemVersion);  
    }

    public static BuildParameters GetParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var target = context.Argument("target", "Default");
        var buildSystem = context.BuildSystem();

        var workflow = buildSystem.GitHubActions.Environment.Workflow;
        var isTagged = workflow.RefType == Cake.Common.Build.GitHubActions.Data.GitHubActionsRefType.Tag;

        return new BuildParameters {
            Target = target,
            Configuration = context.Argument("configuration", "Release"),
            IsLocalBuild = buildSystem.IsLocalBuild,            
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsCI = buildSystem.GitHubActions.IsRunningOnGitHubActions,
            IsPullRequest = buildSystem.GitHubActions.Environment.PullRequest.IsPullRequest,
            IsMainRepo = StringComparer.OrdinalIgnoreCase.Equals("nhibernate/fluent-nhibernate", workflow.Repository),
            // tag builds are cut from main
            IsMainBranch = isTagged || StringComparer.OrdinalIgnoreCase.Equals("main", workflow.RefName),
            IsTagged = isTagged,
            GitHub = BuildGitHub.GetWithCredentials(context, "nhibernate", "fluent-nhibernate"),
            NuGet = BuildNuGet.GetWithCredentials(context),
            ReleaseNotes = context.ParseReleaseNotes("./ReleaseNotes.md"),
            IsPublishBuild = IsPublishing(target),
            IsReleaseBuild = IsReleasing(target)                          
        };
    }

    private static bool IsReleasing(string target)
    {
        var targets = new [] { "Publish", "Publish-NuGet", "Publish-GitHub-Release" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }

    private static bool IsPublishing(string target)
    {
        var targets = new [] { "Release-Notes", "Create-Release-Notes" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }
}
