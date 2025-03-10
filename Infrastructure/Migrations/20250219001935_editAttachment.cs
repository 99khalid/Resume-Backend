using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_ResumeId",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ResumeId",
                table: "Attachments",
                column: "ResumeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_ResumeId",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ResumeId",
                table: "Attachments",
                column: "ResumeId");
        }
    }
}
