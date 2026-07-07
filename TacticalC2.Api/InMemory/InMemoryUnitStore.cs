using TacticalC2.Domain.Entities;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Api.InMemory;

public class InMemoryUnitStore
{
    public List<Unit> Units { get; } =
    [
        Unit.Create("Drone-Alpha-1", UnitType.Drone, 50.06, 19.94, 90, 12),
        Unit.Create("Vehicle-Bravo-1", UnitType.Vehicle, 50.07, 19.95, 180, 8),
        Unit.Create("Infantry-Charlie-1", UnitType.Infantry, 50.05, 19.93, 45, 2)
    ];
}