using Net.Essentials;
using System.Reflection;
using Xamarin.Forms;
using Net.UI;

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