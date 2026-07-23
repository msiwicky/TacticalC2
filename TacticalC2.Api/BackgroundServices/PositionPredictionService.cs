using Microsoft.AspNetCore.SignalR;
using TacticalC2.Api.Hubs;
using TacticalC2.Domain.Enums;
using TacticalC2.Infrastructure.Persistence;

namespace TacticalC2.Api.BackgroundServices;

public class PositionPredictionService(
    IServiceScopeFactory scopeFactory,
    IHubContext<UnitsHub> hubContext,
    ILogger<PositionPredictionService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PredictAndBroadcast();
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task PredictAndBroadcast()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TacticalDbContext>();

        var now = DateTime.UtcNow;
        var units = dbContext.Units.ToList();

        foreach (var unit in units)
        {
            var status = unit.CalculateStatus(now);

            if (status == UnitStatus.Active)
                continue;

            var (predictedLat, predictedLng) = unit.PredictCurrentPosition(now);

            await hubContext.Clients.Group("units-subscribers").SendAsync(
                "UnitPositionPredicted",
                new
                {
                    UnitId = unit.Id,
                    Latitude = predictedLat,
                    Longitude = predictedLng,
                    Status = status.ToString()
                });
        }
    }
}