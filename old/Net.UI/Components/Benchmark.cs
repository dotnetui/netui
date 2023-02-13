using System.Diagnostics;

namespace Net.Essentials;

public class Benchmark : IDisposable
{
    public event EventHandler<Benchmark> Stopped;

    public DateTime Start { get; private set; }
    public DateTime? End { get; private set; }
    public TimeSpan Elapsed => watch.Elapsed;
    public string[] Tags { get; private set; }

    readonly Stopwatch watch = new();

    public Benchmark(params string[] tags)
    {
        Tags = tags;
        watch.Start();
        Start = DateTime.Now;
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }

    public TimeSpan Stop()
    {
        if (!watch.IsRunning) return watch.Elapsed;
        watch.Stop();
        End = DateTime.Now;
        Stopped?.Invoke(this, this);
        return watch.Elapsed;
    }

    public string StopToString()
    {
        Stop();
        return ToString();
    }

    public override string ToString()
    {
        return watch.Elapsed.TotalSeconds.ToString("N2") + "s";
    }
}