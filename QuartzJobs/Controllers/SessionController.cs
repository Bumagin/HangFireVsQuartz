using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzJobs.Jobs.SessionJob;

namespace QuartzJobs.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SessionController : ControllerBase
{
    private readonly ISchedulerFactory _factory;
    private readonly ILogger<SessionController> _logger;

    public SessionController(
        ISchedulerFactory factory, 
        ILogger<SessionController> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task StartSession(string client)
    {
        _logger.LogInformation($"Клиент {client} начал сессию в {DateTime.Now}");
        
        var scheduler = await _factory.GetScheduler();

        // Создание JobDataMap с параметрами
        var jobDataMap = new JobDataMap
        {
            { "Client", client } // Передача параметра через запрос
        };

        // Создание JobDetail с параметрами и долговечностью
        var jobDetail = JobBuilder.Create<SessionJob>()
            .WithIdentity(nameof(SessionJob)) // Уникальный идентификатор
            .UsingJobData(jobDataMap) // Передача параметров
            .StoreDurably() // Обозначение задачи как долговечной
            .Build();

        // Убедитесь, что задача зарегистрирована в планировщике
        await scheduler.AddJob(jobDetail, true); // true означает, что задача будет заменять существующую задачу с таким же идентификатором

        // Создание триггера для выполнения задачи через 25 секунд
        var trigger = TriggerBuilder.Create()
            .ForJob(jobDetail)
            .StartAt(DateBuilder.FutureDate(25, IntervalUnit.Second)) // Запуск через 25 секунд
            .Build();

        // Запуск задачи с триггером
        await scheduler.ScheduleJob(trigger); 
    }
}