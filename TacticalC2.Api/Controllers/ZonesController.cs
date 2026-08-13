using MediatR;
using Microsoft.AspNetCore.Mvc;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Application.Zones.Commands.CreateZone;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonesController(IMediator mediator,IGeofenceZoneRepository repository) : ControllerBase
{
    public record CreateZoneRequest(string Name, List<PointDto> BoundaryPoints);
    public record ZoneResponse(Guid Id, string Name, List<PointDto> BoundaryPoints);
    public record PointDto(double Latitude, double Longitude);

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateZoneRequest request)
    {
        var points = request.BoundaryPoints.Select(p => (p.Latitude, p.Longitude)).ToList();
        var id = await mediator.Send(new CreateZoneCommand(request.Name, points));
        return Ok(id);
    }
    [HttpGet]
    public async Task<ActionResult<List<GeofenceZone>>> GetAll()
    {
        var zones = await repository.GetAllAsync();
        
        var response = zones.Select(z => new ZoneResponse(
            z.Id,
            z.Name,
            z.BoundaryPoints.Select(p => new PointDto(p.Latitude, p.Longitude)).ToList()
        )).ToList();
        
        return Ok(response);
    }
}