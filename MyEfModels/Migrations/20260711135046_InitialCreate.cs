using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    TitleRomanji = table.Column<string>(type: "text", nullable: false),
                    TitleEnglish = table.Column<string>(type: "text", nullable: true),
                    TitleOriginal = table.Column<string>(type: "text", nullable: true),
                    TitleSynonyms = table.Column<List<string>>(type: "text[]", nullable: false),
                    MainPictureMedium = table.Column<string>(type: "text", nullable: true),
                    MainPictureLarge = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MeanScore = table.Column<float>(type: "real", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Popularity = table.Column<int>(type: "integer", nullable: false),
                    ListUserCount = table.Column<int>(type: "integer", nullable: false),
                    ScoringUserCount = table.Column<int>(type: "integer", nullable: false),
                    Nsfw = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MediaType = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    NumberEpisodes = table.Column<int>(type: "integer", nullable: true),
                    StartSeasonYear = table.Column<int>(type: "integer", nullable: true),
                    StartSeasonSeason = table.Column<string>(type: "text", nullable: true),
                    BroadcastWeekday = table.Column<string>(type: "text", nullable: true),
                    BroadcastTime = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: true),
                    EpisodeDuration = table.Column<int>(type: "integer", nullable: true),
                    rating = table.Column<string>(type: "text", nullable: true),
                    Pictures = table.Column<string[]>(type: "text[]", nullable: false),
                    BackgroundInformation = table.Column<string>(type: "text", nullable: true),
                    WatchingCount = table.Column<int>(type: "integer", nullable: false),
                    CompletedCount = table.Column<int>(type: "integer", nullable: false),
                    OnHoldCount = table.Column<int>(type: "integer", nullable: false),
                    DroppedCount = table.Column<int>(type: "integer", nullable: false),
                    PlanToWatchCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MalId = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: true),
                    JoinDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anime1Id = table.Column<int>(type: "integer", nullable: false),
                    Anime2Id = table.Column<int>(type: "integer", nullable: false),
                    votes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeRecommendations_Animes_Anime1Id",
                        column: x => x.Anime1Id,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimeRecommendations_Animes_Anime2Id",
                        column: x => x.Anime2Id,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Anime1Id = table.Column<int>(type: "integer", nullable: false),
                    Anime2Id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeRelations_Animes_Anime1Id",
                        column: x => x.Anime1Id,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimeRelations_Animes_Anime2Id",
                        column: x => x.Anime2Id,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeGenres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeGenres_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeStudios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    StudioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeStudios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeStudios_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeStudios_Studios_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    WatchedEpisodes = table.Column<int>(type: "integer", nullable: false),
                    IsRewatching = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserList_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenres_AnimeId",
                table: "AnimeGenres",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenres_GenreId",
                table: "AnimeGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRecommendations_Anime1Id_Anime2Id",
                table: "AnimeRecommendations",
                columns: new[] { "Anime1Id", "Anime2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRecommendations_Anime2Id",
                table: "AnimeRecommendations",
                column: "Anime2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelations_Anime1Id_Anime2Id",
                table: "AnimeRelations",
                columns: new[] { "Anime1Id", "Anime2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelations_Anime2Id",
                table: "AnimeRelations",
                column: "Anime2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_MalId",
                table: "Animes",
                column: "MalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_MediaType",
                table: "Animes",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_TitleRomanji",
                table: "Animes",
                column: "TitleRomanji",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStudios_AnimeId",
                table: "AnimeStudios",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeStudios_StudioId",
                table: "AnimeStudios",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_UserList_AnimeId",
                table: "UserList",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserList_Status",
                table: "UserList",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserList_UserId",
                table: "UserList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeGenres");

            migrationBuilder.DropTable(
                name: "AnimeRecommendations");

            migrationBuilder.DropTable(
                name: "AnimeRelations");

            migrationBuilder.DropTable(
                name: "AnimeStudios");

            migrationBuilder.DropTable(
                name: "UserList");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Studios");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
