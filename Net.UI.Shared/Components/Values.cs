using Net.Essentials.Systems;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public static class Values
    {
        public static Thickness PageMargin => NotchSystem.Instance.GetPageMargin();
        public static Thickness TopMargin => NotchSystem.Instance.TopMargin;
        public static Thickness BottomMargin => NotchSystem.Instance.BottomMargin;
        public static PopCommand PopCommand => new PopCommand();
    }
}
