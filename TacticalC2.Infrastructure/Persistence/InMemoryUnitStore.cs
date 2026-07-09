using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Infrastructure.Persistence;

public class InMemoryUnitStore:IUnitRepository
{
    private readonly List<Unit> _units = [];

    public Unit? GetById(Guid id) => _units.FirstOrDefault(u => u.Id == id);

    public IEnumerable<Unit> GetAll() => _units;
    public void Add(Unit unit) => _units.Add(unit);
}