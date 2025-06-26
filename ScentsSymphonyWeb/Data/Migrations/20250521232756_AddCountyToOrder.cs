using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScentsSymphonyWeb.Data.Migrations
{
    public partial class AddCountyToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "County",
                table: "Orders");
        }
    }
}
