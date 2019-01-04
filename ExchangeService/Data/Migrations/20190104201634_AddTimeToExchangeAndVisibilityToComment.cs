using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddTimeToExchangeAndVisibilityToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstUserFinalizeTime",
                table: "Exchanges",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondUserFinalizeTime",
                table: "Exchanges",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstUserFinalizeTime",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "SecondUserFinalizeTime",
                table: "Exchanges");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Comments");
        }
    }
}
