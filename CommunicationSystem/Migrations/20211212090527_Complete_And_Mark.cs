using Microsoft.EntityFrameworkCore.Migrations;

namespace CommunicationSystem.Migrations
{
    public partial class Complete_And_Mark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UsersToTests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "UsersToTests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UsersToTests");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "UsersToTests");
        }
    }
}
