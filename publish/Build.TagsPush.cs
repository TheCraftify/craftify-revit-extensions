using Octokit;

#pragma warning disable
sealed partial class Build
{
    Target PushTags =>
        _ =>
            _.TriggeredBy(NugetPush)
                .Requires(() => GitHubToken)
                .Requires(() => GitRepository)
                .Requires(() => GitGlobalUserEmail)
                .Executes(async () =>
                {
                    var referenceName = $"refs/tags/{MinorVersion}";
                    var gitHubName = GitRepository.GetGitHubName();
                    var gitHubOwner = GitRepository.GetGitHubOwner();
                    var commitSha = GitRepository.Commit;

                    try
                    {
                        var existingTag = await GitHubTasks.GitHubClient.Git.Reference.Get(
                            gitHubOwner,
                            gitHubName,
                            referenceName
                        );
                    }
                    catch (NotFoundException)
                    {
                        Log.Information(
                            $"The tag: '{MinorVersion}' already exists. Skipping creation..."
                        );
                        return;
                    }

                    var newTag = new NewTag
                    {
                        Tag = MinorVersion,
                        Message = $"Release {MinorVersion}",
                        Object = commitSha,
                        Type = TaggedType.Commit,
                        Tagger = new Committer(
                            gitHubOwner,
                            GitGlobalUserEmail,
                            DateTimeOffset.UtcNow
                        ),
                    };

                    var tagRef = await GitHubTasks.GitHubClient.Git.Tag.Create(
                        gitHubOwner,
                        gitHubName,
                        newTag
                    );

                    var newReference = new NewReference(referenceName, tagRef.Sha);

                    try
                    {
                        await GitHubTasks.GitHubClient.Git.Reference.Create(
                            gitHubOwner,
                            gitHubName,
                            newReference
                        );
                    }
                    catch (ApiValidationException)
                    {
                        Log.Warning(
                            "The tag's referece {0} already exists. Skipping creation...",
                            referenceName
                        );
                        return;
                    }

                    Log.Information($"✅ Created and pushed tag {MinorVersion}.");
                });
}
