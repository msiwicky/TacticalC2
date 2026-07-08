using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Infrastructure.Persistence;

public class InMemoryUnitStore:IUnitRepository
{
    private readonly List<Unit> _units =
    [
        Unit.Create("Drone-Alpha-1", UnitType.Drone, 50.06, 19.94, 90, 12),
        Unit.Create("Vehicle-Bravo-1", UnitType.Vehicle, 50.07, 19.95, 180, 8),
        Unit.Create("Infantry-Charlie-1", UnitType.Infantry, 50.05, 19.93, 45, 2)
    ];

    public Unit? GetById(Guid id) => _units.FirstOrDefault(u => u.Id == id);

    public IEnumerable<Unit> GetAll() => _units;
}