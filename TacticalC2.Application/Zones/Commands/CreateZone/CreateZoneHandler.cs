using MediatR;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Zones.Commands.CreateZone;

public class CreateZoneHandler(IGeofenceZoneRepository repository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateZoneCommand, Guid>
{
    public async Task<Guid> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = GeofenceZone.Create(request.Name, request.BoundaryPoints);
        
        await repository.AddAsync(zone);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return zone.Id;
    }
}