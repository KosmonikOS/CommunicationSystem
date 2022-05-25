using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class PasswordHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserSaltPassId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserSaltPass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSaltPass", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSaltPass_UserSaltPassId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserSaltPass");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserSaltPassId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserSaltPassId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
