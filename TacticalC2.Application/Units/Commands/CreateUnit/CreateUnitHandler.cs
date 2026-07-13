using MediatR;
using TacticalC2.Application.Common.Interfaces;
using Unit = TacticalC2.Domain.Entities.Unit;

namespace TacticalC2.Application.Units.Commands.CreateUnit;

public class CreateUnitHandler(IUnitRepository repository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateUnitCommand, Guid>
{
    public async Task<Guid> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = Unit.Create(request.Name, request.Type, request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        await repository.AddAsync(unit);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return unit.Id;
    }
}