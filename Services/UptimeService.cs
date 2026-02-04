namespace MyApp.Services;

public class UptimeService
{
    private readonly DateTime _startTime = DateTime.Now;
    public TimeSpan GetUptime() => DateTime.Now - _startTime;
}
