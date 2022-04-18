using System.Diagnostics;

namespace Net;

public class Benchmark
{
    protected readonly Stopwatch watch = new();

    public Benchmark()
    {
        watch.Start();
    }

    public TimeSpan Stop()
    {
        watch.Stop();
        return watch.Elapsed;
    }

    public string StopToString()
    {
        watch.Stop();
        return ToString();
    }

    public override string ToString()
    {
        return watch.Elapsed.TotalSeconds.ToString("N2") + "s";
    }
}
