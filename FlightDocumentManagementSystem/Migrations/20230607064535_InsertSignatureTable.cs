using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightDocumentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InsertSignatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Signature",
                columns: table => new
                {
                    SignatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signature", x => x.SignatureId);
                    table.ForeignKey(
                        name: "FK_Signature_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Signature_DocumentId",
                table: "Signature",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Signature");
        }
    }
}
