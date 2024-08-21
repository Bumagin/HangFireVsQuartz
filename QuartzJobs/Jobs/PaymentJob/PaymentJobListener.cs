using Quartz;
using QuartzJobs.Jobs.PaymentJob;

public class PaymentJobListener : IJobListener
{
    private readonly ILogger<PaymentJobListener> _logger;

    public PaymentJobListener(ILogger<PaymentJobListener> logger)
    {
        _logger = logger;
    }

    public string Name => "PaymentJobListener";

    /// <summary>
    /// Вызывается до выполнения задачи.
    /// </summary>
    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        // Проверка идентификатора задачи
        if (context.JobDetail.Key.Name.Equals(nameof(PaymentJob)))
        {
            _logger.LogInformation("PaymentListener: я услышал что PaymentJob начал роботу!");
        }
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// Вызывается, когда задача была отклонена для выполнения.
    /// </summary>
    public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        // Проверка идентификатора задачи
        if (context.JobDetail.Key.Name.Equals(nameof(PaymentJob)))
        {
            _logger.LogInformation("PaymentListener: я услышал PaymentJob, но меня наложили право вето...");
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Вызывается после выполнения задачи.
    /// </summary>
    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken)
    {
        // Проверка идентификатора задачи
        if (context.JobDetail.Key.Name.Equals(nameof(PaymentJob)))
        {
            _logger.LogInformation($"PaymentListener: PaymentJob выполнен! Отправляю товар клиенту {context.JobDetail.JobDataMap.GetString("Client")}");
        }

        await Task.CompletedTask;
    }
}