using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace TacticalC2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToGeometryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Units\" ALTER COLUMN \"Location\" TYPE geometry(point, 4326) USING \"Location\"::geometry;");

            migrationBuilder.Sql(
                "ALTER TABLE \"GeofenceZones\" ALTER COLUMN \"Boundary\" TYPE geometry(polygon, 4326) USING \"Boundary\"::geometry;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Units\" ALTER COLUMN \"Location\" TYPE geography(point) USING \"Location\"::geography;");

            migrationBuilder.Sql(
                "ALTER TABLE \"GeofenceZones\" ALTER COLUMN \"Boundary\" TYPE geography(polygon) USING \"Boundary\"::geography;");
        }
    }
}
