﻿using System.Windows.Input;

#if XAMARIN
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public class TabBarItem : BindableObject
    {
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public ImageSource SelectedImage
        {
            get => (ImageSource)GetValue(SelectedImageProperty);
            set => SetValue(SelectedImageProperty, value);
        }

        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        public double SelectedOpacity
        {
            get => (double)GetValue(SelectedOpacityProperty);
            set => SetValue(SelectedOpacityProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(TabBarItem),
            defaultValue: null);

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            propertyName: nameof(Image),
            returnType: typeof(ImageSource),
            declaringType: typeof(TabBarItem),
            defaultValue: null);

        public static readonly BindableProperty SelectedImageProperty = BindableProperty.Create(
            propertyName: nameof(SelectedImage),
            returnType: typeof(ImageSource),
            declaringType: typeof(TabBarItem),
            defaultValue: null);

        public static readonly BindableProperty OpacityProperty = BindableProperty.Create(
            propertyName: nameof(Opacity),
            returnType: typeof(double),
            declaringType: typeof(TabBarItem),
            defaultValue: 0.5);

        public static readonly BindableProperty SelectedOpacityProperty = BindableProperty.Create(
            propertyName: nameof(SelectedOpacity),
            returnType: typeof(double),
            declaringType: typeof(TabBarItem),
            defaultValue: 1.0);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(TabBarItem),
            defaultValue: null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(TabBarItem),
            defaultValue: null);
    }
}
