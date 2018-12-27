using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class UpdateExchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "Exchanges",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstUsersGames",
                table: "Exchanges",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherUsersGames",
                table: "Exchanges",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PickUpLocation",
                table: "Exchanges",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pickup",
                table: "Exchanges",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Exchanges",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "FirstUsersGames",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "OtherUsersGames",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "PickUpLocation",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "Pickup",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Exchanges");
        }
    }
}
