using System.Windows.Input;

#if XAMARIN
using System;
using Xamarin.Forms;
#endif

namespace Net.UI
{
    public class Search : SearchBar
    {
        public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(
            nameof(CancelCommand),
            typeof(ICommand),
            typeof(Search),
            null);

        public static readonly BindableProperty CancelCommandParameterProperty = BindableProperty.Create(
            nameof(CancelCommandParameter),
            typeof(object),
            typeof(Search),
            null);

        public static readonly BindableProperty IsCancelVisibleProperty = BindableProperty.Create(
            nameof(IsCancelVisible),
            typeof(bool),
            typeof(Search),
            true);

        public static readonly BindableProperty TextChangeActionProperty = BindableProperty.Create(
            nameof(TextChangeAction),
            typeof(Action<string>),
            typeof(Search),
            null);

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public object CancelCommandParameter
        {
            get => GetValue(CancelCommandParameterProperty);
            set => SetValue(CancelCommandParameterProperty, value);
        }

        public bool IsCancelVisible
        {
            get => (bool)GetValue(IsCancelVisibleProperty);
            set => SetValue(IsCancelVisibleProperty, value);
        }

        public Action<string> TextChangeAction
        {
            get => (Action<string>)GetValue(TextChangeActionProperty);
            set => SetValue(TextChangeActionProperty, value);
        }

        public Search()
        {
            TextChanged += TextChanged2;
        }

        private void TextChanged2(object sender, TextChangedEventArgs e)
        {
            TextChangeAction?.Invoke(e.NewTextValue);
        }
    }
}