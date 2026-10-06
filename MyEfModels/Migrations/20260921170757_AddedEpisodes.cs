using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class AddedEpisodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeEpisodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TitleRomanji = table.Column<string>(type: "text", nullable: true),
                    TitleJapanese = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    Aired = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    Filler = table.Column<bool>(type: "boolean", nullable: false),
                    Recap = table.Column<bool>(type: "boolean", nullable: false),
                    Synopsis = table.Column<string>(type: "text", nullable: true),
                    Replies = table.Column<int>(type: "integer", nullable: true),
                    ForumUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeEpisodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeEpisodes_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisodes_AnimeId_EpisodeNumber",
                table: "AnimeEpisodes",
                columns: new[] { "AnimeId", "EpisodeNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeEpisodes");
        }
    }
}
