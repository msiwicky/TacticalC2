using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;
using TacticalC2.Infrastructure.Configurations;

namespace TacticalC2.Infrastructure.Persistence;

public class TacticalDbContext(DbContextOptions<TacticalDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    private static readonly GeometryFactory GeometryFactory = 
        NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
    public DbSet<Unit> Units => Set<Unit>();
    
    public DbSet<UnitPositionHistory> UnitPositionHistories => Set<UnitPositionHistory>();

    public DbSet<GeofenceZone> GeofenceZones => Set<GeofenceZone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UnitConfiguration());
        modelBuilder.ApplyConfiguration(new UnitPositionHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new GeofenceZoneConfiguration());
    }
    
    public override int SaveChanges()
    {
        SyncGeospatialData();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SyncGeospatialData();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SyncGeospatialData()
    {
        foreach (var entry in ChangeTracker.Entries<Unit>())
        {
            var point = GeometryFactory.CreatePoint(new Coordinate(entry.Entity.Longitude, entry.Entity.Latitude));
            entry.Property("Location").CurrentValue = point;
        }

        foreach (var entry in ChangeTracker.Entries<GeofenceZone>())
        {
            var coordinates = entry.Entity.BoundaryPoints
                .Select(p => new Coordinate(p.Longitude, p.Latitude))
                .ToList();
            
            if (coordinates.Count > 0 && !coordinates[0].Equals(coordinates[^1]))
            {
                coordinates.Add(coordinates[0]);
            }

            var polygon = GeometryFactory.CreatePolygon(coordinates.ToArray());
            entry.Property("Boundary").CurrentValue = polygon;
        }
    }
    
    async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}