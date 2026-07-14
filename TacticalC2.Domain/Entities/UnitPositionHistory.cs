namespace TacticalC2.Domain.Entities;

public class UnitPositionHistory
{
    public Guid Id { get; private set; }
    public Guid UnitId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double Heading { get; private set; }
    public double Speed { get; private set; }
    public DateTime TimestampUtc { get; private set; }

    private UnitPositionHistory() { }

    public static UnitPositionHistory Create(Guid unitId, double latitude, double longitude, double heading, double speed)
    {
        return new UnitPositionHistory
        {
            Id = Guid.NewGuid(),
            UnitId = unitId,
            Latitude = latitude,
            Longitude = longitude,
            Heading = heading,
            Speed = speed,
            TimestampUtc = DateTime.UtcNow
        };
    }
}