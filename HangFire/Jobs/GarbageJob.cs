namespace SharedKernel.Jobs;

public class GarbageJob
{
    private readonly ILogger<GarbageJob> _logger;
    private readonly Random _random = new();

    public GarbageJob(ILogger<GarbageJob> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation($"GarbageJob: удалено {_random.Next(100)} объектов");
    }
}