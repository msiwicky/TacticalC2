using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TacticalC2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPositionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitPositionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Heading = table.Column<double>(type: "double precision", nullable: false),
                    Speed = table.Column<double>(type: "double precision", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitPositionHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitPositionHistories_UnitId_TimestampUtc",
                table: "UnitPositionHistories",
                columns: new[] { "UnitId", "TimestampUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitPositionHistories");
        }
    }
}
