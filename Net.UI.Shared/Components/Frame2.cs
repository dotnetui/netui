using System;
using System.Collections.Generic;
using System.Text;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public class Frame2 : Frame
    {
        public float ShadowRadius
        {
            get => (float)GetValue(ShadowRadiusProperty);
            set => SetValue(ShadowRadiusProperty, value);
        }

        public new bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public static readonly BindableProperty ShadowRadiusProperty =
            BindableProperty.Create(
                nameof(ShadowRadius),
                typeof(float),
                typeof(Frame2),
                -1.0f);

        public static new readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(
                nameof(HasShadow),
                typeof(bool),
                typeof(Frame2),
                false);

        public static readonly BindableProperty ShadowColorProperty =
            BindableProperty.Create(
                nameof(ShadowColor),
                typeof(Color),
                typeof(Frame2),
                Palette.FromHex("#333333"));
    }
}
