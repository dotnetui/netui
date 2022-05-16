namespace Net.Internals;

internal class EmbeddedResourceManager
{
    public static EmbeddedResourceManager Instance { get; private set; } = new EmbeddedResourceManager();
    readonly Assembly assembly = typeof(TitleBar).GetTypeInfo().Assembly;
    readonly ResourceService resourceService = new();

    public ImageSource LoadImage(string name)
    {
        return ImageSource.FromResource(resourceService.FindResourceName(name, assembly), assembly);
    }
}
