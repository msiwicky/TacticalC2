using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Configurations;

public class GeofenceZoneConfiguration : IEntityTypeConfiguration<GeofenceZone>
{
    public void Configure(EntityTypeBuilder<GeofenceZone> builder)
    {
        builder.HasKey(z => z.Id);

        builder.Property(z => z.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Ignore(z => z.BoundaryPoints); 

        builder.Property<Polygon>("Boundary")
            .HasColumnType("geography (polygon)");

        builder.HasIndex("Boundary")
            .HasMethod("GIST");
    }
}