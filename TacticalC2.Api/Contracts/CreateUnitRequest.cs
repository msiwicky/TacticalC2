using TacticalC2.Domain.Enums;

namespace TacticalC2.Api.Contracts;

public record CreateUnitRequest(string Name, UnitType Type, double Latitude, double Longitude, double Heading, double Speed);