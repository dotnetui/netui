using Net.Essentials;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Net.Essentials
{
    public class BenchmarkService : Singleton<BenchmarkService>
    {
        public bool IsEnabled = false;
        public bool IsDebug = false;
        public bool ShouldPrintAfterStops = true;
        public bool Echo = true;
        public uint AutoPrintInterval = 2000;
        public Action<object> LogFunc;

        private readonly ConcurrentDictionary<Benchmark, string> messages = new ConcurrentDictionary<Benchmark, string>();
        private readonly ConcurrentBag<(DateTime Date, TimeSpan Duration, string Message)> finishedTasks = new ConcurrentBag<(DateTime Date, TimeSpan Duration, string Message)>();
        private volatile bool isPrintTimerActive = false;

        void WriteLine(object s)
        {
            if (LogFunc != null)
                LogFunc(s);
            else
                Console.WriteLine(s);
        }

        public IDisposable StartBenchmark([CallerMemberName] string message = null)
        {
            if (!IsEnabled)
                return null;

            var benchmark = new Benchmark();
            messages[benchmark] = message;
            benchmark.Stopped += Benchmark_Stopped;
            if (Echo)
            {
                WriteLine($"[Benchmark Start | {messages.Count} active] {message}");
                StartPrintTimer();
            }
            return benchmark;
        }

        private void Benchmark_Stopped(object sender, Benchmark benchmark)
        {
            benchmark.Stopped -= Benchmark_Stopped;
            StopBenchmark(benchmark);
        }

        public void StopBenchmark(IDisposable handle, string message = null)
        {
            if (handle == null)
                return;

            var benchmark = handle as Benchmark;
            if (messages.TryRemove(benchmark, out var startMessage))
            {
                finishedTasks.Add((DateTime.Now, benchmark.Elapsed, $"{startMessage} {message}"));
                if (Echo)
                {
                    WriteLine($"[Benchmark End | {messages.Count} active] Elapsed: {benchmark.StopToString()} {startMessage} {message}");
                    if (ShouldPrintAfterStops)
                        PrintStatus();
                }
            }
        }

        private void StartPrintTimer()
        {
            Task.Run(async () =>
            {
                if (messages.IsEmpty || isPrintTimerActive)
                    return;

                isPrintTimerActive = true;
                while (isPrintTimerActive && messages.Count > 0)
                {
                    await Task.Delay((int)AutoPrintInterval);
                    PrintStatus();
                }
                isPrintTimerActive = false;
            });
        }

        public void PrintStatus()
        {
            if (!IsEnabled)
                return;

            WriteLine(" ");
            WriteLine("------------------------------------");
            WriteLine("ACTIVE BENCHMARKS");
            foreach (var message in messages)
                WriteLine($"{message.Key}\t{message.Value}");
            WriteLine("------------------------------------");
            WriteLine("RECENT BENCHMARKS");
            foreach (var message in finishedTasks.OrderByDescending(x => x.Date).Take(5))
                WriteLine(message);
            WriteLine("------------------------------------");
            WriteLine("FINISHED BENCHMARKS");
            var summaries = finishedTasks.GroupBy(x => x.Message).Select(x => new
            {
                Duration = TimeSpan.FromSeconds(x.Sum(y => y.Duration.TotalSeconds)),
                Message = x.Key,
                Count = x.Count()
            }).OrderByDescending(x => x.Duration);
            foreach (var item in summaries)
                WriteLine($"{item.Duration}\tx{item.Count,-5}\t\t{item.Duration.TotalSeconds / item.Count:N3} s/t\t\t{item.Message}");
            WriteLine(" ");
        }

        public void Throw(Exception ex, [CallerMemberName] string caller = null)
        {
            Report(ex, caller);
            throw ex;
        }

        public void ThrowIfDebug(Exception ex, [CallerMemberName] string caller = null)
        {
            Report(ex, caller);
#if DEBUG
            throw ex;
#else
        if (IsDebug) throw ex;
#endif
        }

        public virtual void Report(Exception ex, [CallerMemberName] string caller = null)
        {
            WriteLine($"[Exception Thrown by {caller}] {ex}");
        }
    }
}
