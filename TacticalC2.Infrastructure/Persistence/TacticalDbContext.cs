using Microsoft.EntityFrameworkCore;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;
using TacticalC2.Infrastructure.Configurations;

namespace TacticalC2.Infrastructure.Persistence;

public class TacticalDbContext(DbContextOptions<TacticalDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    public DbSet<Unit> Units => Set<Unit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UnitConfiguration());
    }
    
    async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}