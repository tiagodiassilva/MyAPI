using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Services;

namespace MyApp.Pages;

public string CpuUsage { get; private set; } = string.Empty;

public class IndexModel : PageModel
{
    private readonly UptimeService _uptimeService;
    private readonly CpuUsageService _cpuUsageService;

    public IndexModel(UptimeService uptimeService, CpuUsageService cpuUsageService)
    {
        _uptimeService = uptimeService;
        _cpuUsageService = cpuUsageService;
    }

    public string Uptime { get; private set; }

    public void OnGet()
    {
        var uptime = _uptimeService.GetUptime();
        Uptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";

        var cpuUsage = _cpuUsageService.GetCpuUsage();
        CpuUsage = $"{cpuUsage}%";
    }
}
