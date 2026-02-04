using System;
using System.IO;
using System.Linq;

namespace MyApp.Services
{
    public class CpuUsageService
    {
        private long _prevIdle = 0;
        private long _prevTotal = 0;

        public double GetCpuUsage()
        {
            var line = File.ReadLines("/proc/stat").First(l => l.StartsWith("cpu "));
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            long user = long.Parse(parts[1]);
            long nice = long.Parse(parts[2]);
            long system = long.Parse(parts[3]);
            long idle = long.Parse(parts[4]);
            long iowait = long.Parse(parts[5]);
            long irq = long.Parse(parts[6]);
            long softirq = long.Parse(parts[7]);

            long total = user + nice + system + idle + iowait + irq + softirq;

            long totalDiff = total - _prevTotal;
            long idleDiff = idle - _prevIdle;

            double usage = 0;
            if (totalDiff > 0)
            {
                usage = (1.0 - (double)idleDiff / totalDiff) * 100.0;
            }

            _prevTotal = total;
            _prevIdle = idle;

            return usage;
        }
    }
}