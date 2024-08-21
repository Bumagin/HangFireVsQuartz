using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Jobs;

namespace HangFire.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult PayOrder(string client, bool isSuccess)
    {
        _logger.LogInformation("Создал задачу на оплату!");
        
        var jobId = BackgroundJob.Enqueue<PaymentJob>(p => p.Pay(isSuccess));

        BackgroundJob.ContinueJobWith<StoreJob>(jobId, s => s.SendOrder(client));
        
        return Ok();
    }
}