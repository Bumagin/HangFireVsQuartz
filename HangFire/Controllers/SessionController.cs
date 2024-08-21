using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Jobs;

namespace HangFire.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SessionController : ControllerBase
{
    private readonly ILogger<SessionController> _logger;

    public SessionController(ILogger<SessionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult StartSession(string client)
    {
        _logger.LogInformation($"Клиент {client} начал сессию в {DateTime.Now}");

        BackgroundJob.Schedule<SessionJob>(s => s.EndSession(client), TimeSpan.FromSeconds(25));

        return Ok();
    }
}