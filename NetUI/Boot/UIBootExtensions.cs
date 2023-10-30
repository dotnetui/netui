global using Net.UI;
using Net.Essentials;

namespace Net.UI;

public static class UIBootExtensions
{
    public static MauiAppBuilder ConfigureNetUI(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(typeof(BenchmarkService));
        builder.Services.AddSingleton(typeof(ContainerService));
        builder.Services.AddSingleton(typeof(ResourceService));
        builder.Services.AddSingleton(typeof(RandomService));

        DependencyService.RegisterSingleton(BenchmarkService.Instance);
        DependencyService.RegisterSingleton(ContainerService.Instance);
        DependencyService.RegisterSingleton(ResourceService.Instance);
        DependencyService.RegisterSingleton(RandomService.Instance);

        return builder;
    }
}
