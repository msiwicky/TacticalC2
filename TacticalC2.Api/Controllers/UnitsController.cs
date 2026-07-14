using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TacticalC2.Api.Contracts;
using TacticalC2.Api.Hubs;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Application.Units.Commands.CreateUnit;
using TacticalC2.Application.Units.Commands.UpdateUnitPosition;
using TacticalC2.Application.Units.Queries.GetUnitPositionHistory;
using TacticalC2.Domain.Entities;
using Unit = TacticalC2.Domain.Entities.Unit;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController(IUnitRepository repository, IMediator mediator, IHubContext<UnitsHub> hubContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Unit>>> GetAll()
    {
        return Ok(await repository.GetAllAsync());
    }
    [HttpGet("{id:guid}/history")]
    public async Task<ActionResult<List<UnitPositionHistory>>> GetHistory(
        Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var history = await mediator.Send(new GetUnitPositionHistoryQuery(id, from, to));
        return Ok(history);
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateUnitRequest request)
    {
        var id = await mediator.Send(new CreateUnitCommand(request.Name, request.Type, request.Latitude, request.Longitude, request.Heading, request.Speed));
    
        return Ok(id);
    }

    [HttpPut("{id}/position")]
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

        var unit = await repository.GetByIdAsync(id);
        await hubContext.Clients.Group("units-subscribers").SendAsync("UnitPositionUpdated", unit);

        return NoContent();
    }
}