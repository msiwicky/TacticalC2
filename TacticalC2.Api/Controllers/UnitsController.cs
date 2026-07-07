using Microsoft.AspNetCore.Mvc;
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
}