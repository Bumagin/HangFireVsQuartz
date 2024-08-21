using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Jobs;

namespace HangFire.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost]
    public IActionResult SendNotification(string client, string message)
    {
        _logger.LogInformation($"Создал задачу на уведомление клиенту {client}");

        BackgroundJob.Enqueue<NotificationJob>(s => s.SendNotification(client, message));
        
        return Ok();
    }
}