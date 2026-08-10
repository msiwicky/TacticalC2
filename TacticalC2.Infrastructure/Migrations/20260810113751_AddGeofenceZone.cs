using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace TacticalC2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeofenceZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeofenceZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Boundary = table.Column<Polygon>(type: "geography (polygon)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeofenceZones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceZones_Boundary",
                table: "GeofenceZones",
                column: "Boundary")
                .Annotation("Npgsql:IndexMethod", "GIST");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeofenceZones");
        }
    }
}
