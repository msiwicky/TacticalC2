using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Common.Interfaces;

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(Guid id);
    Task<List<Unit>> GetAllAsync();
    Task AddAsync(Unit unit);
}