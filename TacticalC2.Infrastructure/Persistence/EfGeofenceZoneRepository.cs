using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
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

    public async Task<List<GeofenceZone>> GetAllAsync()
    {
        var zones = await dbContext.GeofenceZones.ToListAsync();

        foreach (var zone in zones)
        {
            var boundary = dbContext.Entry(zone).Property<Polygon>("Boundary").CurrentValue;

            var points = boundary.Coordinates
                .Select(c => (Latitude: c.Y, Longitude: c.X))
                .ToList();
            
            zone.RehydrateBoundaryPoints(points);
        }

        return zones;
    }
}