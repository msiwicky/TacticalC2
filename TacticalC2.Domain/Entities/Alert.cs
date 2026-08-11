using TacticalC2.Domain.Enums;

namespace TacticalC2.Domain.Entities;

public class Alert
{
    public Guid Id { get; private set; }
    public Guid UnitId { get; private set; }
    public Guid ZoneId { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public string Message { get; private set; } = "";
    public DateTime TimestampUtc { get; private set; }

    private Alert() { }

    public static Alert Create(Guid unitId, Guid zoneId, AlertSeverity severity, string message)
    {
        return new Alert
        {
            Id = Guid.NewGuid(),
            UnitId = unitId,
            ZoneId = zoneId,
            Severity = severity,
            Message = message,
            TimestampUtc = DateTime.UtcNow
        };
    }
}