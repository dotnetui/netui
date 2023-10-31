using System;
using System.Windows.Input;
#if XAMARIN
using Xamarin.Forms;
#else
using View = Microsoft.Maui.IView;
#endif
namespace Net.UI
{
    public class FocusCommand : ViewModel2, ICommand
    {
        public static FocusCommand Instance { get; } = new FocusCommand();

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is View v)
                v.Focus();
        }
    }
}
