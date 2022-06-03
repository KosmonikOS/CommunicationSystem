using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class Indexes_Test_User_Subject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NickName",
                table: "Users",
                column: "NickName")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Grade",
                table: "Tests",
                column: "Grade")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Name",
                table: "Tests",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Subject_Name",
                table: "Subject",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.Sql("CREATE INDEX \"IX_Users_LastName_FirstName_MiddleName\" " +
                "ON \"Users\" using gin (((((COALESCE(\"LastName\", '') || ' ') || COALESCE(\"FirstName\", '')) || ' ') || COALESCE(\"MiddleName\", '')) gin_trgm_ops)");

            migrationBuilder.Sql("CREATE INDEX \"IX_Users_FullGrage\" " +
                "ON \"Users\" using gin ((COALESCE(\"Grade\"::text, '') || COALESCE(\"GradeLetter\", '')) gin_trgm_ops)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NickName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tests_Grade",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_Name",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Subject_Name",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.Sql("DROP INDEX \"IX_Users_LastName_FirstName_MiddleName\"");

            migrationBuilder.Sql("DROP INDEX \"IX_Users_FullGrage\"");
        }
    }
}
