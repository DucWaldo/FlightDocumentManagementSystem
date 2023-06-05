using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAircraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "Aircraft",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeUpdate",
                table: "Aircraft",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "Aircraft");

            migrationBuilder.DropColumn(
                name: "TimeUpdate",
                table: "Aircraft");
        }
    }
}
