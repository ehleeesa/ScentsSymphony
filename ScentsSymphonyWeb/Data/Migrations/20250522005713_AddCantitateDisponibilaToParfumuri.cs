using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScentsSymphonyWeb.Data.Migrations
{
    public partial class AddCantitateDisponibilaToParfumuri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CantitateDisponibila",
                table: "Perfume",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantitateDisponibila",
                table: "Perfume");
        }
    }
}
