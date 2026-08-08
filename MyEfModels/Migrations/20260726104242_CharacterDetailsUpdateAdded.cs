using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class CharacterDetailsUpdateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeExternalLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeExternalLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeExternalLinks_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenraiUsageCounters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenraiUsageCounters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeExternalLinks_AnimeId",
                table: "AnimeExternalLinks",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeExternalLinks_Url_AnimeId",
                table: "AnimeExternalLinks",
                columns: new[] { "Url", "AnimeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenraiUsageCounters_Date",
                table: "TenraiUsageCounters",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeExternalLinks");

            migrationBuilder.DropTable(
                name: "TenraiUsageCounters");
        }
    }
}
