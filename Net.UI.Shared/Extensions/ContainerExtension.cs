using Net.Essentials;

#if XAMARIN
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#endif

namespace Net.UI
{
    [ContentProperty("Type")]
    public class ContainerExtension : IMarkupExtension
    {
        private readonly ContainerService containerService;

        public Type Type { get; set; }

        public ContainerExtension()
        {
            this.containerService = ContainerService.Instance;
        }

        public object ProvideValue(IServiceProvider _)
        {
            return containerService.GetOrCreate(Type);
        }
    }
}