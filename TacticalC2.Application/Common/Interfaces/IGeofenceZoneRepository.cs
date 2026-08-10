using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Common.Interfaces;

public interface IGeofenceZoneRepository
{
    Task AddAsync(GeofenceZone zone);
    Task<List<GeofenceZone>> GetAllAsync();
}