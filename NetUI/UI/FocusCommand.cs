using System.Windows.Input;

namespace Net.UI
{
    public class FocusCommand : BindableObject, ICommand
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
