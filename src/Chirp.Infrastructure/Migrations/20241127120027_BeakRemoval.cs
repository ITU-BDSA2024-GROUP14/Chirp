using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BeakRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Beak",
                table: "AspNetUsers",
                newName: "DisplayName");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Beak",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_DisplayName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "AspNetUsers",
                newName: "Beak");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_DisplayName",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Beak");
        }
    }
}
