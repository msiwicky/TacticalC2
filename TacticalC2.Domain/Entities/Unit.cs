using TacticalC2.Domain.Enums;

namespace TacticalC2.Domain.Entities;

public class Unit
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public UnitType Type { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double Heading { get; private set; }
    public double Speed { get; private set; }
    public DateTime LastSeenUtc { get; private set; }

    private Unit(string name)
    {
        Name = name;
    }

    public static Unit Create(string name, UnitType type, double latitude, double longitude, double heading,
        double speed)
    {
        return new Unit(name)
        {
            Id = Guid.NewGuid(),
            Type = type,
            Heading = heading,
            Speed = speed,
            Latitude = latitude,
            Longitude = longitude,
            LastSeenUtc = DateTime.UtcNow
        };
    }
    
    public void UpdatePosition(double latitude, double longitude, double heading, double speed)
    {
        Latitude = latitude;
        Longitude = longitude;
        Heading = heading;
        Speed = speed;
        LastSeenUtc = DateTime.UtcNow;
    }

    public UnitStatus CalculateStatus(DateTime now)
    {
        var elapsed = now - LastSeenUtc;
    
        if (elapsed < TimeSpan.FromSeconds(5)) return UnitStatus.Active;
        return elapsed < TimeSpan.FromSeconds(15) ? UnitStatus.Stale : UnitStatus.Offline;
    }
    
    public (double Latitude, double Longitude) PredictCurrentPosition(DateTime now)
    {
        var elapsedSeconds = (now - LastSeenUtc).TotalSeconds;

        if (elapsedSeconds <= 0)
            return (Latitude, Longitude);

        const double metersPerDegree = 111_000;
        var distanceMeters = Speed * elapsedSeconds;
        var distanceDegrees = distanceMeters / metersPerDegree;
        var headingRadians = Heading * Math.PI / 180.0;

        var predictedLat = Latitude + distanceDegrees * Math.Cos(headingRadians);
        var predictedLng = Longitude + distanceDegrees * Math.Sin(headingRadians);

        return (predictedLat, predictedLng);
    }
}