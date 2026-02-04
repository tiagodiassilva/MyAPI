using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Services; // para acessar UptimeService e CpuUsageService

namespace MyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UptimeService _uptimeService;
        private readonly CpuUsageService _cpuUsageService;

        public IndexModel(UptimeService uptimeService, CpuUsageService cpuUsageService)
        {
            _uptimeService = uptimeService;
            _cpuUsageService = cpuUsageService;
        }

        public string Uptime { get; private set; } = string.Empty;
        public string CpuUsage { get; private set; } = string.Empty;

        public void OnGet()
        {
            var uptime = _uptimeService.GetUptime();
            Uptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";

            var cpu = _cpuUsageService.GetCpuUsage();
            CpuUsage = $"{cpu:F2}%";
        }
    }
}
