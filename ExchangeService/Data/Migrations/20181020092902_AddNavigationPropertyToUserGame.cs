using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddNavigationPropertyToUserGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames");
        }
    }
}
