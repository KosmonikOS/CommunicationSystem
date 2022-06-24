using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class ContactUdfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.Sql("CREATE FUNCTION \"GetNotViewedMessages\"(from_id int,to_id int) " +
                "RETURNS int AS ' SELECT COUNT(*) FROM \"Messages\" " +
                "WHERE \"ViewStatus\" = 0 and \"ToId\" = to_id " +
                "and \"FromId\" = from_id' LANGUAGE SQL; ");

            migrationBuilder.Sql("CREATE FUNCTION \"GetLastMessageDate\"(from_id int,to_id int) " +
                "RETURNS timestamp with time zone AS " +
                "' SELECT \"Date\" FROM \"Messages\" WHERE \"Id\" = (SELECT Max(\"Id\") FROM \"Messages\"" +
                " WHERE(\"FromId\" = from_id and \"ToId\" = to_id) or(\"FromId\" = to_id and \"ToId\" = from_id))'" +
                " LANGUAGE SQL; ");

            migrationBuilder.Sql("CREATE FUNCTION \"GetLastMessage\"(from_id int,to_id int) " +
                "RETURNS text AS ' SELECT \"Content\" FROM \"Messages\"" +
                " WHERE \"Id\" = (SELECT Max(\"Id\") FROM \"Messages\" " +
                "WHERE(\"FromId\" = from_id and \"ToId\" = to_id) or(\"FromId\" = to_id and \"ToId\" = from_id))'" +
                " LANGUAGE SQL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.Sql("DROP FUNCTION \"GetNotViewedMessages\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetLastMessageDate\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetLastMessage\"");
        }
    }
}
