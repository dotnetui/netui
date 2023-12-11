using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Net.Essentials.Components
{
    public class CommandModel<T> : ICommand
    {
        public Action<T> ExecuteAction { get; set; }
        public Func<T, bool> CanExecuteFunc { get; set; }

        public event EventHandler CanExecuteChanged;

        public CommandModel(Action<T> execute, Func<T, bool> canExecute = null)
        {
            ExecuteAction = execute;
            CanExecuteFunc = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc((T)parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction?.Invoke((T)parameter);
        }
    }

    public class CommandModel : CommandModel<object>
    {
        public CommandModel(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }
    }
}
