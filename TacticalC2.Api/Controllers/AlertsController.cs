using Microsoft.AspNetCore.Mvc;
using TacticalC2.Application.Common.Interfaces;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController(IAlertRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var alerts = await repository.GetRecentAsync(50);
        return Ok(alerts);
    }
}