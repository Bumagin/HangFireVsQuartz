namespace SharedKernel.Jobs;

public class NotificationJob
{
    private readonly ILogger<NotificationJob> _logger;

    public NotificationJob(ILogger<NotificationJob> logger)
    {
        _logger = logger;
    }

    public async Task SendNotification(string client, string message)
    {
        _logger.LogInformation($"NotificationJob: отправил клиенту {client} сообщение {message}");
    }
}