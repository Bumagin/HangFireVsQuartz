﻿using Quartz;

namespace QuartzJobs.Jobs;

[DisallowConcurrentExecution] // Атрибут запрещающий создание более 1 задачи одновременно
public class GarbageJob : IJob, IDisposable
{
    private readonly ILogger<GarbageJob> _logger;
    private readonly Random _random;

    public GarbageJob(ILogger<GarbageJob> logger)
    {
        _logger = logger;
        _random = new Random();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"GarbageJob: очистил {_random.Next(100)} объектов");
        await Task.CompletedTask;
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _logger.LogInformation("GarbageJob: disposing");
    }
}