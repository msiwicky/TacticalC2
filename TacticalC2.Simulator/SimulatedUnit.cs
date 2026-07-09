namespace TacticalC2.Simulator;

public class SimulatedUnit
{
    public required Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }
    
    public void Move(double deltaSeconds)
    {
        const double metersPerDegree = 111_000;
        
        var distanceMeters = Speed * deltaSeconds;
        var distanceDegrees = distanceMeters / metersPerDegree;

        var headingRadians = Heading * Math.PI / 180.0;

        Latitude += distanceDegrees * Math.Cos(headingRadians);
        Longitude += distanceDegrees * Math.Sin(headingRadians);
    }
}