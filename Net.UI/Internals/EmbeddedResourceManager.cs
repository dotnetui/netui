using Net.Essentials.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Internals
{
    internal class EmbeddedResourceManager
    {
        public static EmbeddedResourceManager Instance { get; private set; } = new EmbeddedResourceManager();
        readonly Assembly assembly = typeof(TitleBar).GetTypeInfo().Assembly;
        readonly ResourceService resourceService = new ResourceService();

        public ImageSource LoadImage(string name)
        {
            return ImageSource.FromResource(resourceService.FindResourceName(name, assembly), assembly);
        }
    }
}
