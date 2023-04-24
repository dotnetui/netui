using Net.Essentials.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
