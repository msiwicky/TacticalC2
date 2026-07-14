using MediatR;
using TacticalC2.Application.Common.Interfaces;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Application.Units.Queries.GetUnitPositionHistory;

public class GetUnitPositionHistoryHandler(IUnitPositionHistoryRepository repository) 
    : IRequestHandler<GetUnitPositionHistoryQuery, List<UnitPositionHistory>>
{
    public Task<List<UnitPositionHistory>> Handle(GetUnitPositionHistoryQuery request, CancellationToken cancellationToken)
    {
        return repository.GetByUnitIdAsync(request.UnitId, request.From, request.To);
    }
}