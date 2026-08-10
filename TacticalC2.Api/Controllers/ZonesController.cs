using MediatR;
using Microsoft.AspNetCore.Mvc;
using TacticalC2.Application.Zones.Commands.CreateZone;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonesController(IMediator mediator) : ControllerBase
{
    public record CreateZoneRequest(string Name, List<PointDto> BoundaryPoints);
    public record PointDto(double Latitude, double Longitude);

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateZoneRequest request)
    {
        var points = request.BoundaryPoints.Select(p => (p.Latitude, p.Longitude)).ToList();
        var id = await mediator.Send(new CreateZoneCommand(request.Name, points));
        return Ok(id);
    }
}