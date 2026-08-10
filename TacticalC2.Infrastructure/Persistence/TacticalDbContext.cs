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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UnitConfiguration());
        modelBuilder.ApplyConfiguration(new UnitPositionHistoryConfiguration());
    }
    
    public override int SaveChanges()
    {
        SyncUnitLocations();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SyncUnitLocations();
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private void SyncUnitLocations()
    {
        foreach (var entry in ChangeTracker.Entries<Unit>())
        {
            var point = GeometryFactory.CreatePoint(new Coordinate(entry.Entity.Longitude, entry.Entity.Latitude));
            entry.Property("Location").CurrentValue = point;
        }
    }
    
    async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}