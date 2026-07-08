using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TacticalC2.Api.Contracts;
using TacticalC2.Api.Hubs;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Application.Units.Commands.UpdateUnitPosition;
using Unit = TacticalC2.Domain.Entities.Unit;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController(IUnitRepository repository, IMediator mediator, IHubContext<UnitsHub> hubContext) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Unit>> GetAll()
    {
        return Ok(repository.GetAll());
    }

    [HttpPut("{id:guid}/position")]
    public async Task<ActionResult> UpdatePosition(Guid id, [FromBody] UpdatePositionRequest request)
    {
        try
        {
            await mediator.Send(new UpdateUnitPositionCommand(id, request.Latitude, request.Longitude, request.Heading, request.Speed));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        var unit = repository.GetById(id);
        await hubContext.Clients.Group("units-subscribers").SendAsync("UnitPositionUpdated", unit);

        return NoContent();
    }
}