## How to release

### Ensure tokens are current

Ensure that GitHub and NuGet tokens are not expired. If they are update `GITHUB_TOKEN` and `NUGET_API_KEY` environment variables in AppVeyor.

### Prepare milestone

1. Create milestone
2. Assign issues to the milestone
3. Label issues
4. Create release notes
    ```ps
    .\build.ps1 --target Release-Notes
    ```

### Tag a release

1. Tag the version and push tag to the upstream
