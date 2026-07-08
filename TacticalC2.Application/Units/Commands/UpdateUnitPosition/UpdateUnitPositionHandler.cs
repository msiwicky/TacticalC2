using MediatR;
using TacticalC2.Application.Common.Interfaces;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public class UpdateUnitPositionHandler(IUnitRepository repository) : IRequestHandler<UpdateUnitPositionCommand>
{
    public Task Handle(UpdateUnitPositionCommand request, CancellationToken cancellationToken)
    {
        var unit = repository.GetById(request.UnitId);
        
        if (unit is null)
            throw new KeyNotFoundException($"Unit {request.UnitId} not found");

        unit.UpdatePosition(request.Latitude, request.Longitude, request.Heading, request.Speed);

        return Task.CompletedTask;
    }
}