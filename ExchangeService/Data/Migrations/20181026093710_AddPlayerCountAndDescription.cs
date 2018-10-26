using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddPlayerCountAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserGameDescription",
                table: "UserGames",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinAgeRequired",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGameDescription",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "MinAgeRequired",
                table: "Games");
        }
    }
}
