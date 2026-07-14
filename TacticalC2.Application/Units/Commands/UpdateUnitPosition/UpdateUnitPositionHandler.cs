using MediatR;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public class UpdateUnitPositionHandler(IUnitRepository repository, 
    IUnitPositionHistoryRepository historyRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUnitPositionCommand>
{
    public async Task Handle(UpdateUnitPositionCommand request, CancellationToken cancellationToken)
    {
        var unit = await repository.GetByIdAsync(request.UnitId);
        
        if (unit is null)
            throw new KeyNotFoundException($"Unit {request.UnitId} not found");

        unit.UpdatePosition(request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        var historyEntry = UnitPositionHistory.Create(
            request.UnitId, request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        await historyRepository.AddAsync(historyEntry);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}