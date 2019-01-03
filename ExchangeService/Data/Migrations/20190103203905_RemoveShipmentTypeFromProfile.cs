using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class RemoveShipmentTypeFromProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Pickup",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "PickUpLocation",
                table: "UserProfiles",
                newName: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "UserProfiles",
                newName: "PickUpLocation");

            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "UserProfiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pickup",
                table: "UserProfiles",
                nullable: false,
                defaultValue: false);
        }
    }
}
