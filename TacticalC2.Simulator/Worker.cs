using System.Net.Http.Json;

namespace TacticalC2.Simulator;

public class Worker(ILogger<Worker> logger,TacticalApiClient apiClient) : BackgroundService
{
    private List<SimulatedUnit> _units = [];

    private static List<SimulatedUnit> CreateInitialUnits()
    {
        return
        [
            new SimulatedUnit
            {
                Id = Guid.NewGuid(), Name = "Drone-Alpha-1", Type = "Drone", Latitude = 50.06, Longitude = 19.94,
                Heading = 90, Speed = 12
            },
            new SimulatedUnit
            {
                Id = Guid.NewGuid(), Name = "Vehicle-Bravo-1", Type = "Vehicle", Latitude = 50.07, Longitude = 19.95,
                Heading = 180, Speed = 8
            },
            new SimulatedUnit
            {
                Id = Guid.NewGuid(), Name = "Infantry-Charlie-1", Type = "Infantry", Latitude = 50.05,
                Longitude = 19.93, Heading = 45, Speed = 2
            },
            new SimulatedUnit
            {
                Id = Guid.NewGuid(), Name = "Drone-Delta-1", Type = "Drone", Latitude = 50.08, Longitude = 19.96,
                Heading = 270, Speed = 14
            },
            new SimulatedUnit
            {
                Id = Guid.NewGuid(), Name = "Vehicle-Echo-1", Type = "Vehicle", Latitude = 50.04, Longitude = 19.92,
                Heading = 0, Speed = 10
            }
        ];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _units = await InitializeUnits();

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var unit in _units)
            {
                unit.Move(deltaSeconds: 1.0);
                
                if (unit.IsConnectionLost)
                {
                    logger.LogWarning("{Name}: connection lost", unit.Name);
                    continue;
                }
                
                await apiClient.SendPositionUpdateAsync(unit);
                logger.LogInformation("{Name}: {Lat:F5}, {Lng:F5}", unit.Name, unit.Latitude, unit.Longitude);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
    
    private async Task<List<SimulatedUnit>> InitializeUnits()
    {
        var desiredUnits = CreateInitialUnits();
        var existingUnits = await apiClient.GetExistingUnitsAsync();

        foreach (var unit in desiredUnits)
        {
            var existing = existingUnits.FirstOrDefault(e => e.Name == unit.Name);

            if (existing is not null)
            {
                logger.LogInformation("Reusing existing unit: {Name} ({Id}) at last known position", unit.Name, existing.Id);
                unit.Id = existing.Id;
                unit.Latitude = existing.Latitude;
                unit.Longitude = existing.Longitude;
                unit.Heading = existing.Heading;
                unit.Speed = existing.Speed;
            }
            else
            {
                logger.LogInformation("Registering new unit: {Name}", unit.Name);
                unit.Id = await apiClient.RegisterUnit(unit);
            }
        }

        return desiredUnits;
    }
}
