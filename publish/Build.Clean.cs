#pragma warning disable
sealed partial class Build
{
    Target Clean =>
        _ =>
            _.TriggeredBy(AddNugetSource)
                .Executes(() =>
                {
                    CleanDirectory(ArtifactsDirectory);

                    foreach (
                        var project in Solution.AllProjects.Where(project =>
                            project != Solution.publish.Build
                        )
                    )
                    {
                        CleanDirectory(project.Directory / "bin");
                        CleanDirectory(project.Directory / "obj");
                    }
                });

    static void CleanDirectory(AbsolutePath path)
    {
        Log.Information("Cleaning directory: {Directory}", path);
        path.CreateOrCleanDirectory();
    }
}
