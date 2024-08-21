using Quartz;

namespace QuartzJobs.Jobs;

public class NotificationJob : IJob
{
    private readonly ILogger<NotificationJob> _logger;

    public NotificationJob(ILogger<NotificationJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var clientName = context.JobDetail.JobDataMap.GetString("Client");

        _logger.LogInformation($"NotificationLog: отправил уведомление клиенту {clientName}");
    }
}