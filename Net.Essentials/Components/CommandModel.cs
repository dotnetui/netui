using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Net.Essentials
{
    public class CommandModel<T> : ICommand
    {
        Func<Task<T>> ExecuteTask { get; set; }
        Action<T> ExecuteAction { get; set; }
        Func<T, bool> CanExecuteFunc { get; set; }

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
        public CommandModel(Action execute, Func<object, bool> canExecute = null, bool waitUntilDone = true) : base(_ => execute?.Invoke(), canExecute, waitUntilDone)
        {
        }

        public CommandModel(Func<Task> execute, Func<object, bool> canExecute = null, bool waitUntilDone = true) : base(() => ExecuteAsyncWrapper(execute), canExecute, waitUntilDone)
        {
        }

        static async Task<object> ExecuteAsyncWrapper(Func<Task> execute)
        {
            if (execute != null)
                await execute();
            return null;
        }
    }
}
