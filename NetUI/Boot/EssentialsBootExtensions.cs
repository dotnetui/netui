global using Net.Essentials;

namespace Net.Essentials;

public static class EssentialsBootExtensions
{
    public static MauiAppBuilder ConfigureNetEssentials(this MauiAppBuilder builder)
    {
        DependencyService.Register<BenchmarkService>();
        DependencyService.Register<ContainerService>();
        DependencyService.Register<ResourceService>();
        DependencyService.Register<RandomService>();

        builder.Services.AddSingleton(DependencyService.Get<BenchmarkService>());
        builder.Services.AddSingleton(DependencyService.Get<ContainerService>());
        builder.Services.AddSingleton(DependencyService.Get<ResourceService>());
        builder.Services.AddSingleton(DependencyService.Get<RandomService>());

        return builder;
    }
}
