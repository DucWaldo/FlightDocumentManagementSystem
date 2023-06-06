using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixAirportCoutryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coutry",
                table: "Airport",
                newName: "Country");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Airport",
                newName: "Coutry");
        }
    }
}
