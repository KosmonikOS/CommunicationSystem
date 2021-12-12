using Microsoft.EntityFrameworkCore.Migrations;

namespace CommunicationSystem.Migrations
{
    public partial class QuestionImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Questions",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Questions");
        }
    }
}
