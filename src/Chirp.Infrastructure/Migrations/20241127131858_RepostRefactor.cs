using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RepostRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Cheeps",
                newName: "_text"
                );
            
            migrationBuilder.AlterColumn<string>(
                    name: "_text",
                    table: "Cheeps",
                    nullable: true);


            migrationBuilder.AddColumn<int>(
                name: "ContentCheepId",
                table: "Cheeps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Cheeps",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_ContentCheepId",
                table: "Cheeps",
                column: "ContentCheepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Cheeps_ContentCheepId",
                table: "Cheeps",
                column: "ContentCheepId",
                principalTable: "Cheeps",
                principalColumn: "CheepId",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.Sql("UPDATE Cheeps SET Type = 'OriginalCheep'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Cheeps_ContentCheepId",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_ContentCheepId",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "ContentCheepId",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "_text",
                table: "Cheeps");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Cheeps",
                type: "TEXT",
                maxLength: 160,
                nullable: false,
                defaultValue: "");
        }
    }
}
