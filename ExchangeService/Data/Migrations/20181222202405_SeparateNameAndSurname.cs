using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class SeparateNameAndSurname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAndSurname",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserProfiles",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "UserProfiles",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "NameAndSurname",
                table: "UserProfiles",
                maxLength: 264,
                nullable: false,
                defaultValue: "");
        }
    }
}
