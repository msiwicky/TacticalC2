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
}