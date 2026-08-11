using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Message).IsRequired().HasMaxLength(500);
        builder.HasIndex(a => a.TimestampUtc);
    }
}