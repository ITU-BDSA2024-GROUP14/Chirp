using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BeakDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Beak");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Name",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Beak");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Beak",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Beak",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Name");
        }
    }
}
