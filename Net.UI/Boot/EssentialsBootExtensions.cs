using Net.Essentials;

namespace Net.Essentials;

public static class EssentialsBootExtensions
{
    public static MauiAppBuilder ConfigureNetEssentials(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ContainerService>();
        builder.Services.AddSingleton<ResourceService>();
        builder.Services.AddSingleton<RandomService>();

        return builder;
    }
}
