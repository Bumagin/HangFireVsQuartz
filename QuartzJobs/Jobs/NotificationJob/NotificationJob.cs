using Quartz;

namespace QuartzJobs.Jobs;

public class NotificationJob : IJob
{
    private readonly ILogger<NotificationJob> _logger;

    public NotificationJob(ILogger<NotificationJob> logger)
    {
        _logger = logger;
    }
    
    public string ClientName { get; set; }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"NotificationLog: отправил уведомление клиенту {ClientName}");
    }
}