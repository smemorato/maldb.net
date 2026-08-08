using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "LastTenraiUpdate",
                table: "Animes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "LinkId",
                table: "AnimeExternalLinks",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTenraiUpdate",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "LinkId",
                table: "AnimeExternalLinks");
        }
    }
}
