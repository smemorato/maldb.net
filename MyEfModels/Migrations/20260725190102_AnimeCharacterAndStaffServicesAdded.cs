using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class AnimeCharacterAndStaffServicesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime1Id",
                table: "AnimeRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime2Id",
                table: "AnimeRecommendations");

            migrationBuilder.RenameColumn(
                name: "votes",
                table: "AnimeRecommendations",
                newName: "Votes");

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NameKanji = table.Column<string>(type: "text", nullable: true),
                    Nicknames = table.Column<List<string>>(type: "text[]", nullable: false),
                    Favorites = table.Column<int>(type: "integer", nullable: false),
                    About = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    WebSite = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    GivenName = table.Column<string>(type: "text", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    AlternateNames = table.Column<List<string>>(type: "text[]", nullable: false),
                    Birthday = table.Column<string>(type: "text", nullable: true),
                    Favorites = table.Column<int>(type: "integer", nullable: false),
                    About = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Favorites = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeCharacters_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeCharacters_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeStaff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeStaff_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeStaff_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeCharacterVoiceActors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeCharacterId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeCharacterVoiceActors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeCharacterVoiceActors_AnimeCharacters_AnimeCharacterId",
                        column: x => x.AnimeCharacterId,
                        principalTable: "AnimeCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeCharacterVoiceActors_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacters_AnimeId_CharacterId",
                table: "AnimeCharacters",
                columns: new[] { "AnimeId", "CharacterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacters_CharacterId",
                table: "AnimeCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacterVoiceActors_AnimeCharacterId_PersonId_Language",
                table: "AnimeCharacterVoiceActors",
                columns: new[] { "AnimeCharacterId", "PersonId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacterVoiceActors_PersonId",
                table: "AnimeCharacterVoiceActors",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStaff_AnimeId_PersonId_Position",
                table: "AnimeStaff",
                columns: new[] { "AnimeId", "PersonId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStaff_PersonId",
                table: "AnimeStaff",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_MalId",
                table: "Characters",
                column: "MalId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_MalId",
                table: "Persons",
                column: "MalId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Name",
                table: "Persons",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime1Id",
                table: "AnimeRecommendations",
                column: "Anime1Id",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime2Id",
                table: "AnimeRecommendations",
                column: "Anime2Id",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime1Id",
                table: "AnimeRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime2Id",
                table: "AnimeRecommendations");

            migrationBuilder.DropTable(
                name: "AnimeCharacterVoiceActors");

            migrationBuilder.DropTable(
                name: "AnimeStaff");

            migrationBuilder.DropTable(
                name: "AnimeCharacters");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.RenameColumn(
                name: "Votes",
                table: "AnimeRecommendations",
                newName: "votes");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime1Id",
                table: "AnimeRecommendations",
                column: "Anime1Id",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeRecommendations_Animes_Anime2Id",
                table: "AnimeRecommendations",
                column: "Anime2Id",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
