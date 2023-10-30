using Net.Essentials;

#if XAMARIN
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#endif

namespace Net.UI
{
    [ContentProperty("Key")]
    public class LExtension : IMarkupExtension
    {
        public string Key { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return L.T(Key);
        }
    }

    [ContentProperty("Key")]
    public class LuExtension : IMarkupExtension
    {
        public string Key { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return L.T(Key)?.ToUpper();
        }
    }
}