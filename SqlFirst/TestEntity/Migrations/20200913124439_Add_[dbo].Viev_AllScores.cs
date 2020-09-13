using Microsoft.EntityFrameworkCore.Migrations;

namespace TestEntity.Migrations
{
    public partial class Add_dboViev_AllScores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spShowSomeStudents = "CREATE VIEW [dbo].Viev_AllScores " +
                    "AS SELECT sub.[Name] AS SubjectName, co.[Name] AS CourseName, spec.[Name] AS SpecialtyName " +
                    "FROM [dbo].[Subjects] sub " +
                    "JOIN [dbo].[SubjectCourses] subCo ON subCo.SubjectId = sub.Id " +
                    "JOIN [dbo].[Courses] co ON co.Id = subCo.CourseId " +
                    "JOIN [dbo].[SubjectSpecialties] subSpec ON subSpec.SubjectId = sub.Id " +
                    "JOIN [dbo].[Specialties] spec ON spec.Id = subSpec.SpecialtyId";

            migrationBuilder.Sql(spShowSomeStudents);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var spShowSomeStudents = "DROP VIEW [dbo].Viev_AllScores";
            migrationBuilder.Sql(spShowSomeStudents);
        }
    }
}
