using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddBothUsersContactDetailsToExchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpLocation",
                table: "Exchanges");

            migrationBuilder.RenameColumn(
                name: "FirstUsersGames",
                table: "Exchanges",
                newName: "OtherUserContactInfo");

            migrationBuilder.AddColumn<string>(
                name: "OfferingUserContactInfo",
                table: "Exchanges",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfferingUsersGames",
                table: "Exchanges",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferingUserContactInfo",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "OfferingUsersGames",
                table: "Exchanges");

            migrationBuilder.RenameColumn(
                name: "OtherUserContactInfo",
                table: "Exchanges",
                newName: "FirstUsersGames");

            migrationBuilder.AddColumn<string>(
                name: "PickUpLocation",
                table: "Exchanges",
                nullable: true);
        }
    }
}
