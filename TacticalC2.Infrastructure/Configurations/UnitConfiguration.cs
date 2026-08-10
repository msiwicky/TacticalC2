using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Configurations;

public class UnitConfiguration:IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
       builder.HasKey(x => x.Id);
       
       builder.Property(u => u.Name)
           .IsRequired()
           .HasMaxLength(200);
       
       builder.Property<Point>("Location")
           .HasColumnType("geometry (point, 4326)");

       builder.HasIndex("Location")
           .HasMethod("GIST");
    }
}