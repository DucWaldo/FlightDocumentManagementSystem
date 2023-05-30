using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangePerrmissionAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Document_DocumentId",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Permission",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_DocumentId",
                table: "Permission",
                newName: "IX_Permission_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Category_CategoryId",
                table: "Permission",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Category_CategoryId",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Permission",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_CategoryId",
                table: "Permission",
                newName: "IX_Permission_DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Document_DocumentId",
                table: "Permission",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
