using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class SaltPassFk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSaltPass_UserSaltPassId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserSaltPassId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserSaltPassId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserSaltPass",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserSaltPass_UserId",
                table: "UserSaltPass",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSaltPass_Users_UserId",
                table: "UserSaltPass",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSaltPass_Users_UserId",
                table: "UserSaltPass");

            migrationBuilder.DropIndex(
                name: "IX_UserSaltPass_UserId",
                table: "UserSaltPass");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSaltPass");

            migrationBuilder.AddColumn<int>(
                name: "UserSaltPassId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserSaltPassId",
                table: "Users",
                column: "UserSaltPassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSaltPass_UserSaltPassId",
                table: "Users",
                column: "UserSaltPassId",
                principalTable: "UserSaltPass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
