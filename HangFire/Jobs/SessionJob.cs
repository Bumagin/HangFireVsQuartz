namespace SharedKernel.Jobs;

public class SessionJob
{
    private readonly ILogger<SessionJob> _logger;

    public SessionJob(ILogger<SessionJob> logger)
    {
        _logger = logger;
    }

    public async Task EndSession(string client)
    {
        _logger.LogInformation($"SessionJob: активная сессия клиента {client} закончилась в {DateTime.Now}");
    }
}