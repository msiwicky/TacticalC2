using MediatR;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Units.Queries.GetUnitPositionHistory;

public record GetUnitPositionHistoryQuery(Guid UnitId, DateTime From, DateTime To) 
    : IRequest<List<UnitPositionHistory>>;