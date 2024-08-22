using Microsoft.AspNetCore.Mvc;
using Quartz.Spi;

namespace QuartzJobs.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class QuartzController : ControllerBase
{
    private readonly IJobStore _jobStore;
    
    public QuartzController(IJobStore jobStore)
    {
        _jobStore = jobStore;
    }

    [HttpPost]
    public async Task<IActionResult> StopAllJobs(CancellationToken cancellationToken)
    {
        await _jobStore.PauseAll(cancellationToken);

        return Ok();
    }
}