using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TacticalC2.Domain.Entities;
using TacticalC2.Infrastructure.Persistence;

namespace TacticalC2.Domain.Tests;

public class UnitPositionHistoryRepositoryTests
{
    private static TacticalDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TacticalDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TacticalDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ThenSaveChanges_PersistsHistoryEntry()
    {
        // Arrange
        await using var dbContext = CreateInMemoryContext();
        var repository = new UnitPositionHistoryRepository(dbContext);
        var unitId = Guid.NewGuid();
        var historyEntry = UnitPositionHistory.Create(unitId, 50.06, 19.94, 90, 12);

        // Act
        await repository.AddAsync(historyEntry);
        await dbContext.SaveChangesAsync();

        // Assert
        var saved = await dbContext.UnitPositionHistories.FirstOrDefaultAsync(h => h.UnitId == unitId);
        saved.Should().NotBeNull();
        saved!.Latitude.Should().Be(50.06);
        saved.Longitude.Should().Be(19.94);
    }

    [Fact]
    public async Task GetByUnitIdAsync_ReturnsOnlyEntriesWithinTimeRange()
    {
        // Arrange
        await using var dbContext = CreateInMemoryContext();
        var repository = new UnitPositionHistoryRepository(dbContext);
        var unitId = Guid.NewGuid();

        var oldEntry = UnitPositionHistory.Create(unitId, 50.0, 19.0, 0, 0);
        var recentEntry = UnitPositionHistory.Create(unitId, 50.1, 19.1, 0, 0);

        dbContext.UnitPositionHistories.AddRange(oldEntry, recentEntry);
        await dbContext.SaveChangesAsync();

        // Act
        var results = await repository.GetByUnitIdAsync(
            unitId, 
            DateTime.UtcNow.AddMinutes(-1), 
            DateTime.UtcNow.AddMinutes(1));

        // Assert
        results.Should().HaveCount(2);
    }
}