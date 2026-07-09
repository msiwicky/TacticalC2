using MediatR;
using TacticalC2.Application.Common.Interfaces;
using Unit = TacticalC2.Domain.Entities.Unit;

namespace TacticalC2.Application.Units.Commands.CreateUnit;

public class CreateUnitHandler(IUnitRepository repository):IRequestHandler<CreateUnitCommand,Guid>
{
    public Task<Guid> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = Unit.Create(request.Name, request.Type, request.Latitude, request.Longitude, request.Heading, request.Speed);
        
        repository.Add(unit);

        return Task.FromResult(unit.Id);
    }
}