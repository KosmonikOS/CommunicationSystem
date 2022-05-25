using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class PasswordHash_Delete_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSaltPass");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserSaltPass",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
