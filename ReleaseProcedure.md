## How to release

### Ensure publishing is configured

Releasing runs on GitHub Actions (`.github/workflows/dotnet.yml`) when a tag is pushed:

- **NuGet** uses [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing) (OIDC), so there is no long-lived API key to rotate. Ensure a trusted publishing policy exists on nuget.org for the `nhibernate/fluent-nhibernate` repository and the `.NET` workflow, and that the `NUGET_USER` repository secret is set to the owning nuget.org account.
- **GitHub release** uses the built-in `GITHUB_TOKEN`; no secret to maintain.

### Prepare milestone

1. Create milestone
2. Assign issues to the milestone
3. Label issues
4. Create release notes by running the **Prepare Release** workflow (Actions tab → Prepare Release → Run workflow). It creates the draft GitHub release from the milestone.
    - Locally, the equivalent is `.\build.ps1 --target Release-Notes`.

### Tag a release

1. Tag the version and push tag to the upstream
