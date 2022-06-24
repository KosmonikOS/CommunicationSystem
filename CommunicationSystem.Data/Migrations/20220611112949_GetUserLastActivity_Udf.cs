using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationSystem.Data.Migrations
{
    public partial class GetUserLastActivity_Udf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FUNCTION \"GetUserLastActivity\"(user_id int) " +
                                 "RETURNS text AS 'SELECT CASE " +
                                 "WHEN(\"EnterTime\" is not null and \"LeaveTime\" is not null " +
                                    "and \"EnterTime\" <= now() and \"LeaveTime\" <= now()) THEN " +
                                    "CASE " +
                                    "WHEN(\"EnterTime\" > \"LeaveTime\") THEN ''В сети'' " +
                                    "WHEN(\"LeaveTime\"::date = current_date) THEN to_char(\"LeaveTime\",''Был(a) в сети: HH24:MI'') " +
                                    "WHEN(extract(week from \"LeaveTime\") = extract(week from current_date)) THEN to_char(\"LeaveTime\",''Был(a) в сети: TMDay'') " +
                                    "WHEN(extract(year from \"LeaveTime\") = extract(year from current_date)) THEN to_char(\"LeaveTime\",''Был(a) в сети: TMMonth dd'') " +
                                    "ELSE to_char(\"LeaveTime\",''Был(a) в сети: DD.MM.YYYY'') " +
                                    "END " +
                                "ELSE ''Не в сети'' " +
                                "END " +
                                "FROM \"Users\" " +
                                "WHERE \"Id\" = user_id' " +
                                "LANGUAGE SQL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION \"GetUserLastActivity\"");
        }
    }
}
