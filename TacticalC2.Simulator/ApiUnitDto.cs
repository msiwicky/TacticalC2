namespace TacticalC2.Simulator;

public class ApiUnitDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public int Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Heading { get; set; }
    public double Speed { get; set; }
    public DateTime LastSeenUtc { get; set; }
}