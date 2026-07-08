using MediatR;

namespace TacticalC2.Application.Units.Commands.UpdateUnitPosition;

public record UpdateUnitPositionCommand(Guid UnitId, double Latitude, double Longitude, double Heading, double Speed) 
    : IRequest;