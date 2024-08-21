using Quartz;

namespace QuartzJobs.Jobs.SessionJob;

public class SessionJob  : IJob
{
    private readonly ILogger<SessionJob> _logger;

    public SessionJob(ILogger<SessionJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var clientName = context.JobDetail.JobDataMap.GetString("Client");

        _logger.LogInformation($"SessionJob: активная сессия клиента {clientName} закончилась в {DateTime.Now}");
    }
}