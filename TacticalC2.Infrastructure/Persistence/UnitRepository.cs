using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
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
    
    public async Task<List<Guid>> GetZoneIdsContainingUnitAsync(Guid unitId)
    {
        var unit = await dbContext.Units.FirstOrDefaultAsync(u => u.Id == unitId);
        if (unit is null) return [];

        var unitLocation = dbContext.Entry(unit).Property<Point>("Location").CurrentValue;

        return await dbContext.GeofenceZones
            .Where(z => EF.Property<Polygon>(z, "Boundary").Contains(unitLocation))
            .Select(z => z.Id)
            .ToListAsync();
    }
}