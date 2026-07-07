using Microsoft.AspNetCore.Mvc;
using TacticalC2.Api.Contracts;
using TacticalC2.Api.InMemory;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController(InMemoryUnitStore store) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Unit>> GetAll()
    {
        return Ok(store.Units);
    }
    
    [HttpPut("{id:guid}/position")]
    public ActionResult UpdatePosition(Guid id, [FromBody] UpdatePositionRequest request)
    {
        var unit = store.Units.FirstOrDefault(u => u.Id == id);
    
        if (unit is null)
            return NotFound();

        unit.UpdatePosition(request.Latitude, request.Longitude, request.Heading, request.Speed);

        return NoContent();
    }
}