using MediatR;

namespace TacticalC2.Application.Zones.Commands.CreateZone;

public record CreateZoneCommand(string Name, List<(double Latitude, double Longitude)> BoundaryPoints) 
    : IRequest<Guid>;