#if XAMARIN
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
#else
using Microsoft.Maui.Controls.Shapes;
#endif

namespace Net.UI
{
    public partial class Dots
    {
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static readonly BindableProperty CountProperty = 
            BindableProperty.Create(
            nameof(Count), 
            typeof(int), 
            typeof(Dots), 
            1, 
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as Dots).UpdateLayout();
            });

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly BindableProperty SelectedIndexProperty = 
            BindableProperty.Create(
            nameof(SelectedIndex), 
            typeof(int), 
            typeof(Dots), 
            1, 
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as Dots).UpdateLayout();
            });

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly BindableProperty SelectedColorProperty = 
            BindableProperty.Create(
            nameof(SelectedColor),
            typeof(Color), 
            typeof(Dots), 
            Palette.White, 
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as Dots).UpdateLayout();
            });

        public Color NormalColor
        {
            get => (Color)GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }

        public static readonly BindableProperty NormalColorProperty = BindableProperty.Create(
            nameof(NormalColor), typeof(Color), typeof(Dots), Palette.FromHex("#88FFFFFF"), propertyChanged: (bindable, oldVal, newVal) =>
            {
                (bindable as Dots).UpdateLayout();
            });


        public Dots()
        {
            InitializeComponent();
            UpdateLayout();
        }

        void UpdateLayout()
        {
            Children.Clear();

            var size = 10;
            for (int i = 0; i < Count; i++)
            {
                var ellipse = new Ellipse
                {
                    WidthRequest = size,
                    HeightRequest = size,
                    Fill = new SolidColorBrush
                    {
                        Color = i == SelectedIndex ? SelectedColor : NormalColor
                    }
                };
                Children.Add(ellipse);
            }
        }
    }
}