using Microsoft.Extensions.Logging;

namespace SharedKernel.Jobs;

public class StoreJob
{
    private readonly ILogger<StoreJob> _logger;

    public StoreJob(ILogger<StoreJob> logger)
    {
        _logger = logger;
    }

    public void SendOrder(string client)
    {
        _logger.LogInformation($"SendJob: выдал заказ клиенту {client}");
    }
}