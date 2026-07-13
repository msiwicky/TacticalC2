using MediatR;
using TacticalC2.Application.Common.Interfaces;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public class UpdateUnitPositionHandler(IUnitRepository repository, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateUnitPositionCommand>
{
    public async Task Handle(UpdateUnitPositionCommand request, CancellationToken cancellationToken)
    {
        var unit = await repository.GetByIdAsync(request.UnitId);
        
        if (unit is null)
            throw new KeyNotFoundException($"Unit {request.UnitId} not found");

        unit.UpdatePosition(request.Latitude, request.Longitude, request.Heading, request.Speed);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}