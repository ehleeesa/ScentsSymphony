using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScentsSymphonyWeb.Data.Migrations
{
    public partial class AddBrandToParfumuri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Perfume",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Perfume");
        }
    }
}
