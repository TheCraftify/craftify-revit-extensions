#pragma warning disable
sealed partial class Build : NukeBuild
{
    Target NugetPack =>
        _ =>
            _.TriggeredBy(Clean)
                .Executes(() =>
                {
                    foreach (var configuration in Configurations)
                    {
                        var revitVersion = configuration.Split(' ').Last();

                        DotNetPack(options =>
                            options
                                .SetConfiguration(configuration)
                                .SetVersion($"{revitVersion}.{MinorVersion}")
                                .SetOutputDirectory(ArtifactsDirectory / configuration)
                                .SetVerbosity(DotNetVerbosity.minimal)
                        );
                    }
                });
}
