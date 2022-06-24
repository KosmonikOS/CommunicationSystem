using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class Group_Udfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION \"GetGroupLastMessage\"");

            migrationBuilder.Sql("DROP FUNCTION \"GetGroupLastMessageDate\"");
        }
    }
}
