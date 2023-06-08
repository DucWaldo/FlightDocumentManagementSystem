using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixAccountIdInDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Document",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_UserId",
                table: "Document",
                newName: "IX_Document_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Account_AccountId",
                table: "Document",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Account_AccountId",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Document",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_AccountId",
                table: "Document",
                newName: "IX_Document_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
