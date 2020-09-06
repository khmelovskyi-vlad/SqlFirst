using Microsoft.EntityFrameworkCore.Migrations;

namespace TestEntity.Migrations
{
    public partial class sbShowSomeStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spShowSomeStudents = @"CREATE PROCEDURE [dbo].[ShowSomeStudents]
                                    	@studentsCount INT
                                    AS
                                    	SELECT TOP (@studentsCount) *
                                    	FROM [dbo].[Students]
                                    RETURN 0";

            migrationBuilder.Sql(spShowSomeStudents);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var spShowSomeStudents = @"DROP PROCEDURE [dbo].[ShowSomeStudents]";
            migrationBuilder.Sql(spShowSomeStudents);
        }
    }
}
