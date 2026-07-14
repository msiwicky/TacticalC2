using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Common.Interfaces;

public interface IUnitPositionHistoryRepository
{
    Task AddAsync(UnitPositionHistory history);
}