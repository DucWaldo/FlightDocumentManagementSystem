using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePublicUrlInDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicUrl",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicUrl",
                table: "Document");
        }
    }
}
