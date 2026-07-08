using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Common.Interfaces;

public interface IUnitRepository
{
    Unit? GetById(Guid id);
    IEnumerable<Unit> GetAll();
}