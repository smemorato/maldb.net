using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEfModels.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesToUserList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinishDate",
                table: "UserList",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartDate",
                table: "UserList",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "UserList",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "UserList");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "UserList");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "UserList");
        }
    }
}
