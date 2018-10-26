using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeService.Data.Migrations
{
    public partial class AddUserProfileAndComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserSearchGames",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserGames",
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

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReceivingUserId = table.Column<int>(nullable: false),
                    LeavingUserId = table.Column<int>(nullable: false),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(maxLength: 512, nullable: false),
                    Mark = table.Column<double>(nullable: false),
                    UserProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Restrict);
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_UserProfiles_UserProfileId",
                table: "UserGames");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSearchGames_UserProfiles_UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserSearchGames_UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserSearchGames");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserGames");
        }
    }
}
