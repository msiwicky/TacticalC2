using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TacticalC2.Domain.Entities;

namespace TacticalC2.Infrastructure.Configurations;

public class UnitPositionHistoryConfiguration : IEntityTypeConfiguration<UnitPositionHistory>
{
    public void Configure(EntityTypeBuilder<UnitPositionHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.HasIndex(h => new { h.UnitId, h.TimestampUtc });
    }
}