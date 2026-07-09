using MediatR;
using TacticalC2.Domain.Enums;

namespace TacticalC2.Application.Units.Commands.CreateUnit;

public record CreateUnitCommand(string Name, UnitType Type, double Latitude, double Longitude, double Heading, double Speed) 
    : IRequest<Guid>;