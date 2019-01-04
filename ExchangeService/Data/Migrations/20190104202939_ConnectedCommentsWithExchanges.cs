using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class ConnectedCommentsWithExchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondUserFinalizeTime",
                table: "Exchanges",
                newName: "OtherUserFinalizeTime");

            migrationBuilder.RenameColumn(
                name: "FirstUserFinalizeTime",
                table: "Exchanges",
                newName: "OfferingUserFinalizeTime");

            migrationBuilder.AddColumn<int>(
                name: "ConnectedExchangeId",
                table: "Comments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedExchangeId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "OtherUserFinalizeTime",
                table: "Exchanges",
                newName: "SecondUserFinalizeTime");

            migrationBuilder.RenameColumn(
                name: "OfferingUserFinalizeTime",
                table: "Exchanges",
                newName: "FirstUserFinalizeTime");
        }
    }
}
