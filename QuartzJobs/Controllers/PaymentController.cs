using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzJobs.Jobs.PaymentJob;

namespace QuartzJobs.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PaymentController : ControllerBase
{
    private readonly ISchedulerFactory _factory;
    private readonly ILogger<SessionController> _logger;

    public PaymentController(
        ISchedulerFactory factory, 
        ILogger<SessionController> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task PayOrder(string client)
    {
        var scheduler = await _factory.GetScheduler();

        // Создание JobDataMap с параметрами
        var jobDataMap = new JobDataMap
        {
            { "Client", client } // Передача параметра через запрос
        };

        // Создание JobDetail с параметрами и долговечностью
        var jobDetail = JobBuilder.Create<PaymentJob>()
            .WithIdentity(nameof(PaymentJob)) // Уникальный идентификатор
            .UsingJobData(jobDataMap) // Передача параметров
            .StoreDurably() // Обозначение задачи как долговечной
            .Build();

        // Убедитесь, что задача зарегистрирована в планировщике
        await scheduler.AddJob(jobDetail, true); // true означает, что задача будет заменять существующую задачу с таким же идентификатором

        // Создание триггера для выполнения задачи через 25 секунд
        var trigger = TriggerBuilder.Create()
            .ForJob(jobDetail)
            .StartNow()
            .Build();

        // Запуск задачи с триггером
        await scheduler.ScheduleJob(trigger); 
    }
}