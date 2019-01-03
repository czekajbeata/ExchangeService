using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class ChangeShipmentInExchangeToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "Pickup",
                table: "Exchanges");

            migrationBuilder.AddColumn<int>(
                name: "Shipment",
                table: "Exchanges",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipment",
                table: "Exchanges");

            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "Exchanges",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pickup",
                table: "Exchanges",
                nullable: false,
                defaultValue: false);
        }
    }
}
