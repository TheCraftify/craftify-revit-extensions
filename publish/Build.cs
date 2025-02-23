using Octokit;

sealed partial class Build : NukeBuild
{
    const string MinorVersion = "0.0.1";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    string[] Configurations;

    [Parameter]
    string GitGlobalUserEmail;

    [Parameter]
    readonly string NugetSource;

    [Parameter]
    readonly string NugetUserName;

    [Parameter]
    readonly string NugetSourceName;

    [Secret]
    [Parameter]
    readonly string GitHubToken;

    [GitRepository]
    readonly GitRepository GitRepository;

    [Solution(GenerateProjects = true, SuppressBuildProjectCheck = true)]
    readonly Solution Solution;

    public static int Main() => Execute<Build>(x => x.AddNugetSource);

    protected override void OnBuildInitialized()
    {
        Configurations =
        [
            .. Solution
                .Configurations.Select(keyValuePair => keyValuePair.Key)
                .Select(config => config[..config.LastIndexOf('|')])
                .Where(configuration =>
                    configuration.StartsWith("Release ", StringComparison.OrdinalIgnoreCase)
                ),
        ];

        GitGlobalUserEmail = GetGitUserEmail();
        AuthenticateGitHub();
    }

    void AuthenticateGitHub()
    {
        Assert.NotNullOrEmpty(GitHubToken);

        GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
        {
            Credentials = new Credentials(GitHubToken),
        };
    }

    string GetGitUserEmail()
    {
        if (IsServerBuild)
        {
            Assert.NotNullOrWhiteSpace(GitGlobalUserEmail);
            return GitGlobalUserEmail;
        }

        var process = ProcessTasks.StartProcess(
            "git",
            "config --global user.email",
            logOutput: false
        );
        process.AssertWaitForExit();
        var output = process.Output.FirstOrDefault();

        if (
            EqualityComparer<Output>.Default.Equals(output, default)
            || string.IsNullOrWhiteSpace(output.Text)
        )
        {
            Log.Information(
                "Nuke build is running on the GitHub host runner. The default email used in tags creation is: {0}",
                GitGlobalUserEmail
            );
            return GitGlobalUserEmail;
        }

        var email = output.Text.Trim();
        Log.Information("The current user's git email: {0}", email);
        return email;
    }
}
