using Microsoft.EntityFrameworkCore;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Persistence;

public class UnitPositionHistoryRepository(TacticalDbContext dbContext) : IUnitPositionHistoryRepository
{
    public Task AddAsync(UnitPositionHistory history)
    {
        dbContext.UnitPositionHistories.Add(history);
        return Task.CompletedTask;

    }
    public Task<List<UnitPositionHistory>> GetByUnitIdAsync(Guid unitId, DateTime from, DateTime to)
    {
        return dbContext.UnitPositionHistories
            .Where(h => h.UnitId == unitId && h.TimestampUtc >= from && h.TimestampUtc <= to)
            .OrderBy(h => h.TimestampUtc)
            .ToListAsync();
    }
}