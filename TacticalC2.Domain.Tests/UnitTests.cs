using FluentAssertions;
using TacticalC2.Domain.Entities;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Domain.Tests;

public class UnitTests
{
    [Fact]
    public void CalculateStatus_WhenJustSeen_ReturnsActive()
    {
        // Arrange
        var unit = Unit.Create("Drone-Alpha-1", UnitType.Drone, 50.06, 19.94, 90, 12);

        // Act
        var status = unit.CalculateStatus(unit.LastSeenUtc);

        // Assert
        status.Should().Be(UnitStatus.Active);
    }
    [Fact]
    public void CalculateStatus_WhenSeen7SecondsAgo_ReturnsStale()
    {
        // Arrange
        var unit = Unit.Create("Drone-Alpha-1", UnitType.Drone, 50.06, 19.94, 90, 12);

        // Act
        var status = unit.CalculateStatus(unit.LastSeenUtc + TimeSpan.FromSeconds(7));

        // Assert
        status.Should().Be(UnitStatus.Stale);
    }
    
    [Fact]
    public void CalculateStatus_WhenSeen20SecondsAgo_ReturnsOffline()
    {
        // Arrange
        var unit = Unit.Create("Drone-Alpha-1", UnitType.Drone, 50.06, 19.94, 90, 12);

        // Act
        var status = unit.CalculateStatus(unit.LastSeenUtc + TimeSpan.FromSeconds(20));

        // Assert
        status.Should().Be(UnitStatus.Offline);
    }
}