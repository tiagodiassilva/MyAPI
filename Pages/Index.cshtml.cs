using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Services;

namespace MyApp.Pages;

public class IndexModel : PageModel
{
    private readonly UptimeService _uptimeService;

    public IndexModel(UptimeService uptimeService)
    {
        _uptimeService = uptimeService;
    }

    public string Uptime { get; private set; }

    public void OnGet()
    {
        var uptime = _uptimeService.GetUptime();
        Uptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
    }
}
