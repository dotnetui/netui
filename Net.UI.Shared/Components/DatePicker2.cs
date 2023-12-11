using System;
using System.Collections.Generic;
using System.Text;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public class DatePicker2 : DatePicker
    {
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly BindableProperty TextAlignmentProperty = 
            BindableProperty.Create(
            nameof(TextAlignment), 
            typeof(TextAlignment), 
            typeof(DatePicker2), 
            TextAlignment.Start);
    }
}
