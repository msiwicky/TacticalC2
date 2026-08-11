using MediatR;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public class UpdateUnitPositionHandler(IUnitRepository unitRepository, 
    IUnitPositionHistoryRepository historyRepository,
    IAlertRepository alertRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUnitPositionCommand>
{
    public async Task Handle(UpdateUnitPositionCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.GetByIdAsync(request.UnitId);
        
        if (unit is null)
            throw new KeyNotFoundException($"Unit {request.UnitId} not found");

        unit.UpdatePosition(request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        var historyEntry = UnitPositionHistory.Create(
            request.UnitId, request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        await historyRepository.AddAsync(historyEntry);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var zoneIds = await unitRepository.GetZoneIdsContainingUnitAsync(request.UnitId);

        foreach (var zoneId in zoneIds)
        {
            var alreadyAlerted = await alertRepository.ExistsActiveAlertAsync(request.UnitId, zoneId);
            if (alreadyAlerted) continue;

            var alert = Alert.Create(request.UnitId, zoneId, AlertSeverity.Low, 
                $"{unit.Name} entered geofence zone");
            await alertRepository.AddAsync(alert);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}