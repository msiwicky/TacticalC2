namespace TacticalC2.Simulator;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly List<SimulatedUnit> _units = CreateInitialUnits();

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
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var unit in _units)
            {
                logger.LogInformation("{Name}: {Lat}, {Lng}", unit.Name, unit.Latitude, unit.Longitude);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
