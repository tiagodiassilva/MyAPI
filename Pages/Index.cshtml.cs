using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Services; // para acessar UptimeService e CpuUsageService

namespace MyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UptimeService _uptimeService;
        private readonly CpuUsageService _cpuUsageService;
        private readonly MemoryUsageService _memoryUsageService;

        public IndexModel(UptimeService uptimeService, CpuUsageService cpuUsageService, MemoryUsageService memoryUsageService)
        {
            _uptimeService = uptimeService;
            _cpuUsageService = cpuUsageService;
            _memoryUsageService = memoryUsageService;
        }

        public string Uptime { get; private set; } = string.Empty;
        public string CpuUsage { get; private set; } = string.Empty;
        public string MemoryUsage { get; private set; } = string.Empty;

        public void OnGet()
        {
            var uptime = _uptimeService.GetUptime();
            Uptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";

            var cpu = _cpuUsageService.GetCpuUsage();
            CpuUsage = $"{cpu:F2}%";

            var memory = _memoryUsageService.GetMemoryUsage();
            MemoryUsage = $"{memory}";
        }
    }
}
