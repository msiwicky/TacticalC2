namespace TacticalC2.Domain.Entities;

public class GeofenceZone
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = "";
    public IReadOnlyList<(double Latitude, double Longitude)> BoundaryPoints { get; private set; } = [];

    private GeofenceZone() { }

    public static GeofenceZone Create(string name, List<(double Latitude, double Longitude)> boundaryPoints)
    {
        if (boundaryPoints.Count < 3)
            throw new ArgumentException("A zone needs at least 3 points to form a polygon");

        return new GeofenceZone
        {
            Id = Guid.NewGuid(),
            Name = name,
            BoundaryPoints = boundaryPoints
        };
    }
    public void RehydrateBoundaryPoints(List<(double Latitude, double Longitude)> points)
    {
        BoundaryPoints = points;
    }
}