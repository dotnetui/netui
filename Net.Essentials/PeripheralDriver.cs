using System.Text;

namespace Net;

public abstract class PeripheralDriver
{
    public event EventHandler OnDisconnectRequest;

    public Func<byte[], Task> WriteAsyncFunc;
    public Func<string, Task> LogAsyncFunc;

    public virtual TimeSpan Timeout => TimeSpan.FromSeconds(10);
    public virtual TimeSpan RetryWait => TimeSpan.FromMilliseconds(10);

    public virtual Task SetupAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void InjectMessage(byte[] bytes)
    {
    }

    public virtual void InjectMessage(string message)
    {
    }

    protected async Task LogAsync(string message)
    {
        await LogAsyncFunc?.Invoke(message);
    }

    protected async void Log(string message)
    {
        await LogAsync(message);
    }

    protected async Task WaitAndCrashIfFail(Func<bool> boolean)
    {
        if (!await WaitAsync(boolean, Timeout))
            throw new DriverException("Device communication timed out.");
    }

    protected async Task<bool> WaitAsync(Func<bool> condition, TimeSpan? timeout, TimeSpan? wait = null)
    {
        if (wait == null) wait = RetryWait;
        var start = DateTime.Now;
        while (condition())
        {
            await Task.Delay(wait.Value);
            if (timeout.HasValue)
            {
                if (DateTime.Now - start > timeout.Value)
                    return false;
            }
        }
        return true;
    }

    protected virtual async Task WriteAsync(byte[] bytes)
    {
        await WriteAsyncFunc(bytes);
    }

    protected virtual async Task WriteAsync(string message)
    {
        var bytes = Encoding.ASCII.GetBytes(message);
        await WriteAsync(bytes);
    }

    public Task DisconnectAsync()
    {
        OnDisconnectRequest?.Invoke(this, null);
        return Task.CompletedTask;
    }
}

public class DriverException : Exception
{
    public DriverException(string message) : base(message)
    {
    }
}

public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult result);