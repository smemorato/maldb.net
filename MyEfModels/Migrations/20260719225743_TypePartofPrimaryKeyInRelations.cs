using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class TypePartofPrimaryKeyInRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeRelations_Anime1Id_Anime2Id",
                table: "AnimeRelations");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelations_Anime1Id_Anime2Id_Type",
                table: "AnimeRelations",
                columns: new[] { "Anime1Id", "Anime2Id", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeRelations_Anime1Id_Anime2Id_Type",
                table: "AnimeRelations");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelations_Anime1Id_Anime2Id",
                table: "AnimeRelations",
                columns: new[] { "Anime1Id", "Anime2Id" },
                unique: true);
        }
    }
}
