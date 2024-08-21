using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzJobs.Jobs;

namespace QuartzJobs.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotificationController : ControllerBase
{
    private readonly ISchedulerFactory _factory;

    public NotificationController(ISchedulerFactory factory)
    {
        _factory = factory;
    }

    [HttpPost]
    public async Task SendNotification(string client)
    {
        // Создание планировщика
        var scheduler = await _factory.GetScheduler();

        // Создание словаря с параметрами
        var jobDataMap = new JobDataMap
        {
            { "Client", client }
        };

        // Создание задания с параметрами и долговечностью
        var jobDetail = JobBuilder.Create<NotificationJob>()
            .WithIdentity(nameof(NotificationJob)) // Уникальный идентификатор
            .UsingJobData(jobDataMap) // Параметры
            .StoreDurably() // Обозначение задачи как долговечной
            .Build();

        // Добавляем задание в очередь
        await scheduler.AddJob(jobDetail, true); // true означает, что задача будет заменять существующую задачу с таким же идентификатором

        // Создание триггера для немедленного запуска задачи
        var trigger = TriggerBuilder.Create()
            .ForJob(jobDetail)
            .StartNow()
            .Build();

        // Запуск задачи с триггером
        await scheduler.ScheduleJob(trigger); 
    }
}