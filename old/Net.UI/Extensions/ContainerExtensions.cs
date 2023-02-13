namespace Net.UI;

[ContentProperty("Type")]
public class ContainerExtensions : IMarkupExtension
{
    private readonly ContainerService containerService;

    public Type Type { get; set; }

    public ContainerExtensions()
    {
        this.containerService = DependencyService.Get<ContainerService>();
    }

    public object ProvideValue(IServiceProvider _)
    {
        return containerService.GetOrCreate(Type);
    }
}