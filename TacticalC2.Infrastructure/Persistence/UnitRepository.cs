using Microsoft.EntityFrameworkCore;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Persistence;

public class UnitRepository(TacticalDbContext dbContext) : IUnitRepository
{
    public Task<Unit?> GetByIdAsync(Guid id) => dbContext.Units.FirstOrDefaultAsync(u => u.Id == id);

    public Task<List<Unit>> GetAllAsync() => dbContext.Units.ToListAsync();

    public Task AddAsync(Unit unit)
    {
        dbContext.Units.Add(unit);
        return Task.CompletedTask;
    }
}