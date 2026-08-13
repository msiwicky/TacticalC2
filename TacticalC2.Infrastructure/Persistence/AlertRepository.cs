using Microsoft.EntityFrameworkCore;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Persistence;

public class AlertRepository(TacticalDbContext dbContext) : IAlertRepository
{
    public Task AddAsync(Alert alert)
    {
        dbContext.Alerts.Add(alert);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsActiveAlertAsync(Guid unitId, Guid zoneId)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-5);
        
        return dbContext.Alerts.AnyAsync(a => 
            a.UnitId == unitId && a.ZoneId == zoneId && a.TimestampUtc > cutoff);
    }
    
    public Task<List<Alert>> GetRecentAsync(int count)
    {
        return dbContext.Alerts
            .OrderByDescending(a => a.TimestampUtc)
            .Take(count)
            .ToListAsync();
    }
}