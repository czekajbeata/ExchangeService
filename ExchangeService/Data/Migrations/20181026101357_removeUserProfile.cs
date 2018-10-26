using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class removeUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UserProfiles_UserProfileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_UserProfiles_UserProfileId",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSearchGames_UserProfiles_UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserSearchGames_UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserProfileId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserSearchGames",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserGames",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserProfileId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSearchGames_UserProfileId",
                table: "UserSearchGames",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserProfileId",
                table: "Comments",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UserProfiles_UserProfileId",
                table: "Comments",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_UserProfiles_UserProfileId",
                table: "UserGames",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSearchGames_UserProfiles_UserProfileId",
                table: "UserSearchGames",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
