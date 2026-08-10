using Microsoft.EntityFrameworkCore;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Persistence;

public class EfGeofenceZoneRepository(TacticalDbContext dbContext) : IGeofenceZoneRepository
{
    public Task AddAsync(GeofenceZone zone)
    {
        dbContext.GeofenceZones.Add(zone);
        return Task.CompletedTask;
    }

    public Task<List<GeofenceZone>> GetAllAsync() => dbContext.GeofenceZones.ToListAsync();
}