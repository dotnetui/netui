using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Net.Essentials
{
    public class CommandModel<T> : ICommand
    {
        public Func<Task<T>> ExecuteTask { get; set; }
        public Action<T> ExecuteAction { get; set; }
        public Func<T, bool> CanExecuteFunc { get; set; }

        public event EventHandler CanExecuteChanged;
        public bool WaitUntilDone { get; set; }

        volatile bool isExecuting = false;

        public CommandModel(Action<T> execute, Func<T, bool> canExecute = null, bool waitUntilDone = true)
        {
            ExecuteAction = execute;
            CanExecuteFunc = canExecute;
            WaitUntilDone = waitUntilDone;
        }

        public CommandModel(Func<Task<T>> execute, Func<T, bool> canExecute = null, bool waitUntilDone = true)
        {
            ExecuteTask = execute;
            CanExecuteFunc = canExecute;
            WaitUntilDone = waitUntilDone;
        }


        public bool CanExecute(object parameter)
        {
            return !isExecuting && (CanExecuteFunc == null || CanExecuteFunc((T)parameter));
        }

        public void Execute(object parameter)
        {
            if (isExecuting && WaitUntilDone) return;

            isExecuting = WaitUntilDone;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            if (ExecuteTask != null) Task.Run(async () =>
            {
                try
                {
                    await ExecuteTask();
                }
                catch { throw; }
                finally
                {
                    isExecuting = false;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }

            });
            else
            {
                ExecuteAction?.Invoke((T)parameter);
                isExecuting = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public class CommandModel : CommandModel<object>
    {
        public CommandModel(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }
    }
}
