#pragma warning disable
sealed partial class Build
{
    Target NugetPush =>
        _ =>
            _.TriggeredBy(NugetPack)
                .Requires(() => GitHubToken)
                .Requires(() => NugetSource)
                .Executes(() =>
                {
                    foreach (var package in ArtifactsDirectory.GlobFiles("**/*.nupkg"))
                    {
                        DotNetNuGetPush(options =>
                            options
                                .SetTargetPath(package)
                                .SetSource(NugetSource)
                                .SetApiKey(GitHubToken)
                                .SetSkipDuplicate(true)
                        );

                        Log.Information(
                            "Successfully pushed package: {0}, for configuration: {1}",
                            package,
                            Directory.GetParent(package)
                        );
                    }
                });
}
