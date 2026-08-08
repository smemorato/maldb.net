using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstrainFromAnimeTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Animes_TitleRomanji",
                table: "Animes");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_TitleRomanji",
                table: "Animes",
                column: "TitleRomanji");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Animes_TitleRomanji",
                table: "Animes");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_TitleRomanji",
                table: "Animes",
                column: "TitleRomanji",
                unique: true);
        }
    }
}
