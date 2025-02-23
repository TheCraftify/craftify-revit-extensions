using Octokit;

#pragma warning disable
sealed partial class Build
{
    Target CreateGitHubRelease =>
        _ =>
            _.TriggeredBy(PushTags)
                .Requires(() => GitHubToken)
                .Requires(() => GitRepository)
                .Executes(async () =>
                {
                    var gitHubName = GitRepository.GetGitHubName();
                    var gitHubOwner = GitRepository.GetGitHubOwner();

                    var existingReleases = await GitHubTasks.GitHubClient.Repository.Release.GetAll(
                        gitHubOwner,
                        gitHubName
                    );

                    if (existingReleases.Any(release => release.TagName.Equals(MinorVersion)))
                    {
                        Log.Warning($"Release '{MinorVersion}' already exists. Skipping creation.");
                        return;
                    }

                    var artifacts = ArtifactsDirectory.GlobFiles("**/*.nupkg");
                    Assert.NotEmpty(artifacts, "No artifacts were found to create the Release");

                    var newRelease = new NewRelease(MinorVersion)
                    {
                        Name = MinorVersion,
                        TargetCommitish = GitRepository.Commit,
                        Prerelease =
                            MinorVersion.Contains("-beta")
                            || MinorVersion.Contains("-dev")
                            || MinorVersion.Contains("-preview"),
                    };

                    var release = await GitHubTasks.GitHubClient.Repository.Release.Create(
                        gitHubOwner,
                        gitHubName,
                        newRelease
                    );

                    await UploadArtifactsAsync(release, artifacts);
                });

    static async Task UploadArtifactsAsync(
        Release createdRelease,
        IEnumerable<AbsolutePath> artifacts
    )
    {
        foreach (var file in artifacts)
        {
            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(file),
                RawData = File.OpenRead(file),
            };

            await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(
                createdRelease,
                releaseAssetUpload
            );
            Log.Information("Artifact: {Path} is uploaded", file);
        }
    }
}
