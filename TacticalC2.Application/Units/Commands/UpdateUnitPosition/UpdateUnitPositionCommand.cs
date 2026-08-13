using MediatR;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public record UpdateUnitPositionCommand(Guid UnitId, double Latitude, double Longitude, double Heading, double Speed) 
    : IRequest<List<Alert>>;