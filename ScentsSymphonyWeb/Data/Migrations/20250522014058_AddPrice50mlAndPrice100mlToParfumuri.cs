using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScentsSymphonyWeb.Data.Migrations
{
    public partial class AddPrice50mlAndPrice100mlToParfumuri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Perfume");

            migrationBuilder.AddColumn<int>(
                name: "Price100ml",
                table: "Perfume",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price50ml",
                table: "Perfume",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price100ml",
                table: "Perfume");

            migrationBuilder.DropColumn(
                name: "Price50ml",
                table: "Perfume");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Perfume",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
