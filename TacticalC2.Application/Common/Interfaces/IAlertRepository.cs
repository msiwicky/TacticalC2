using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Common.Interfaces;

public interface IAlertRepository
{
    Task AddAsync(Alert alert);
    Task<bool> ExistsActiveAlertAsync(Guid unitId, Guid zoneId);
    Task<List<Alert>> GetRecentAsync(int count);
}