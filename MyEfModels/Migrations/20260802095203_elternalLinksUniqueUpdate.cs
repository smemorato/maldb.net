using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class elternalLinksUniqueUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeExternalLinks_Url_AnimeId",
                table: "AnimeExternalLinks");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeExternalLinks_Url_AnimeId_Name",
                table: "AnimeExternalLinks",
                columns: new[] { "Url", "AnimeId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeExternalLinks_Url_AnimeId_Name",
                table: "AnimeExternalLinks");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeExternalLinks_Url_AnimeId",
                table: "AnimeExternalLinks",
                columns: new[] { "Url", "AnimeId" },
                unique: true);
        }
    }
}
