using Quartz;

namespace QuartzJobs.Jobs.PaymentJob;

public class PaymentJob : IJob
{
    private readonly ILogger<PaymentJob> _logger;

    public PaymentJob(ILogger<PaymentJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("PaymentJob: обращаюсь к банку...");

        // Задержка для имитации работы
        await Task.Delay(5000);

        _logger.LogInformation("PaymentJob: платеж потверждаю!");
    }
}