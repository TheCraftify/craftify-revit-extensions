#pragma warning disable
sealed partial class Build
{
    Target AddNugetSource =>
        _ =>
            _.Requires(() => NugetSource)
                .Requires(() => NugetSourceName)
                .Requires(() => GitHubToken)
                .Requires(() => NugetUserName)
                .Requires(() => GitRepository)
                .Executes(() =>
                {
                    var existingSources = ProcessTasks
                        .StartProcess("dotnet", "nuget list source", logOutput: true)
                        .AssertWaitForExit()
                        .Output.Select(output => output.Text.Trim())
                        .ToList();

                    if (existingSources.Contains(NugetSource))
                    {
                        Log.Information(
                            "NuGet source '{0}' already exists, skipping...",
                            NugetSource
                        );

                        return;
                    }

                    DotNetNuGetAddSource(options =>
                        options
                            .SetSource(NugetSource)
                            .SetUsername(
                                IsServerBuild ? NugetUserName : GitRepository.GetGitHubOwner()
                            )
                            .SetName(NugetSourceName)
                            .SetPassword(GitHubToken)
                            .EnableStorePasswordInClearText()
                    );

                    Log.Information("NuGet source '{0}' added successfully.", NugetSource);
                });
}
