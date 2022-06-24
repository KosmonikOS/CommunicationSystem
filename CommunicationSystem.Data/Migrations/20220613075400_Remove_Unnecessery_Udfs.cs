using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class Remove_Unnecessery_Udfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP FUNCTION \"GetLastMessageDate\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetLastMessage\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetGroupLastMessage\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetGroupLastMessageDate\"");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.Sql("CREATE FUNCTION \"GetGroupLastMessage\"(group_id uuid)" +
                "RETURNS text AS ' SELECT \"Content\" FROM \"Messages\"" +
                " WHERE \"Id\" = (SELECT Max(\"Id\") FROM \"Messages\" " +
                "WHERE \"ToGroup\" = group_id)'" +
                " LANGUAGE SQL; ");

            migrationBuilder.Sql("CREATE FUNCTION \"GetGroupLastMessageDate\"(group_id uuid)" +
                "RETURNS timestamp with time zone AS ' SELECT \"Date\" FROM \"Messages\"" +
                " WHERE \"Id\" = (SELECT Max(\"Id\") FROM \"Messages\"" +
                " WHERE \"ToGroup\" = group_id)'" +
                " LANGUAGE SQL; ");
        }
    }
}
