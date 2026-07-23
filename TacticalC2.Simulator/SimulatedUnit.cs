namespace TacticalC2.Simulator;

public class SimulatedUnit
{
    private static readonly Random Random = new();
    public bool IsConnectionLost { get; private set; }
    private int _ticksUntilReconnect;

    public required Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }
    
    public void Move(double deltaSeconds)
    {
        if (IsConnectionLost)
        {
            _ticksUntilReconnect--;
            if (_ticksUntilReconnect <= 0)
            {
                IsConnectionLost = false;
            }
            
            DriftHeading(maxDegreesPerTick: 8.0);
            MoveForward(deltaSeconds);
            return; 
        }
        
        MaybeStartConnectionLoss();
        
        DriftHeading(maxDegreesPerTick: 2.0);
        MoveForward(deltaSeconds);
        
        ApplyGpsNoise();
    }
    private void MoveForward(double deltaSeconds)
    {
        const double metersPerDegree = 111_000;
        var distanceMeters = Speed * deltaSeconds;
        var distanceDegrees = distanceMeters / metersPerDegree;
        var headingRadians = Heading * Math.PI / 180.0;

        Latitude += distanceDegrees * Math.Cos(headingRadians);
        Longitude += distanceDegrees * Math.Sin(headingRadians);
    }
    
    private void DriftHeading(double maxDegreesPerTick)
    {
        var change = (Random.NextDouble() * 2 - 1) * maxDegreesPerTick;
        Heading = (Heading + change + 360) % 360;
    }
    
    private void ApplyGpsNoise()
    {
        const double maxNoiseMeters = 3.0;
        const double metersPerDegree = 111_000;

        var noiseLatMeters = (Random.NextDouble() * 2 - 1) * maxNoiseMeters;
        var noiseLngMeters = (Random.NextDouble() * 2 - 1) * maxNoiseMeters;

        Latitude += noiseLatMeters / metersPerDegree;
        Longitude += noiseLngMeters / metersPerDegree;
    }
    
    private void MaybeStartConnectionLoss()
    {
        const double lossChancePerTick = 0.005;

        if (!(Random.NextDouble() < lossChancePerTick)) return;
        IsConnectionLost = true;
        _ticksUntilReconnect = Random.Next(5, 20);
    }
}