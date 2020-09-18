public class BuildGitHub
{    
    public string Owner { get; private set; }
    public string Repository { get; private set; }
    public string Url => $"https://github.com/{Owner}/{Repository}";
    public string UserName { get; private set; }
    public string Password { get; private set; }

    public BuildGitHub(
        string owner,
        string repository,
        string userName, 
        string password)
    {
        Owner = owner;
        Repository = repository;
        UserName = userName;
        Password = password;
    }

    public static BuildGitHub GetWithCredentials(ICakeContext context, string owner, string repository)
    {
        var username = context.EnvironmentVariable("GITHUB_USERNAME");
        // if (string.IsNullOrEmpty(username))
        // {
        //     throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
        // }
        
        var token = context.EnvironmentVariable("GITHUB_TOKEN");
        // if (string.IsNullOrEmpty(token))
        // {
        //     throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
        // }

        return new BuildGitHub(owner, repository, username, token);            
    }   
}

public class BuildNuGet
{    
    public string ApiUrl { get; private set; }
    public string ApiKey { get; private set; }

    public BuildNuGet(string apiUrl, string apiKey)
    {        
        ApiUrl = apiUrl;
        ApiKey = apiKey;
    }

    public static BuildNuGet GetWithCredentials(ICakeContext context)
    {
        var apiUrl = context.EnvironmentVariable("NUGET_API_URL");
        // if(string.IsNullOrEmpty(apiUrl)) 
        // {
        //     throw new InvalidOperationException("Could not resolve NuGet API url.");
        // }
        
        var apiKey = context.EnvironmentVariable("NUGET_API_KEY");
        // if(string.IsNullOrEmpty(apiKey)) 
        // {
        //     throw new InvalidOperationException("Could not resolve NuGet API key.");
        // }

        return new BuildNuGet(apiUrl, apiKey);
    }
}



