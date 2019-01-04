using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddTimeToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameTimeInMin",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameTimeInMin",
                table: "Games");
        }
    }
}
