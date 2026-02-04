namespace MyApp.Services;

public class MemoryUsageService
{
    public string GetMemoryUsage()
    {
        // Em Linux, podemos usar GC.GetTotalMemory para memória usada pelo processo .NET
        // ou ler /proc/meminfo para memória total do sistema.
        // Aqui vamos mostrar o uso do processo atual.

        long memoryBytes = GC.GetTotalMemory(forceFullCollection: false);
        double memoryMB = memoryBytes / 1024.0 / 1024.0;

        return $"{memoryMB:F2} MB";
    }
}
