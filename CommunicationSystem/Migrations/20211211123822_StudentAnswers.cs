using Microsoft.EntityFrameworkCore.Migrations;

namespace CommunicationSystem.Migrations
{
    public partial class StudentAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_studentAnswers",
                table: "studentAnswers");

            migrationBuilder.RenameTable(
                name: "studentAnswers",
                newName: "StudentAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.RenameTable(
                name: "StudentAnswers",
                newName: "studentAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentAnswers",
                table: "studentAnswers",
                column: "Id");
        }
    }
}
