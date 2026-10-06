using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class AddedReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<string>(type: "text", nullable: true),
                    Review = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true),
                    Tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    Overall = table.Column<int>(type: "integer", nullable: true),
                    Nice = table.Column<int>(type: "integer", nullable: true),
                    LoveIt = table.Column<int>(type: "integer", nullable: true),
                    Funny = table.Column<int>(type: "integer", nullable: true),
                    Confusing = table.Column<int>(type: "integer", nullable: true),
                    Informative = table.Column<int>(type: "integer", nullable: true),
                    WellWritten = table.Column<int>(type: "integer", nullable: true),
                    Creative = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeReviews_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReviews_AnimeId",
                table: "AnimeReviews",
                column: "AnimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeReviews");
        }
    }
}
