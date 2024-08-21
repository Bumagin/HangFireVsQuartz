using Hangfire;

namespace SharedKernel.Jobs;

public class PaymentJob
{
    private readonly ILogger<PaymentJob> _logger;

    public PaymentJob(ILogger<PaymentJob> logger)
    {
        _logger = logger;
    }

    // Атрибут количества попыток выполнения HangFire
    [AutomaticRetry(Attempts = 3)] 
    public async Task Pay(bool isSuccess)
    {
        _logger.LogInformation($"PaymentJob: сделал запрос к банку...");
        Thread.Sleep(3000);

        if (!isSuccess)
            throw new ArgumentException("PaymentJob: ошибка перевода!");
            
        _logger.LogInformation($"PaymentJob: потверждаю перевод!");
    }
}