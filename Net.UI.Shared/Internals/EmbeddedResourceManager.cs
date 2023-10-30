using Net.Essentials;
using System.Reflection;
using Net.UI;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.Internals
{
    internal class EmbeddedResourceManager
    {
        public static EmbeddedResourceManager Instance { get; private set; } = new EmbeddedResourceManager();
        readonly Assembly assembly = typeof(Application2).GetTypeInfo().Assembly;
        readonly ResourceService resourceService = new ResourceService();

        public ImageSource LoadImage(string name)
        {
            return ImageSource.FromResource(resourceService.FindResourceName(name, assembly), assembly);
        }
    }
}