using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AdoNetClient
{
    class QueryRepository
    {
        public Dictionary<string, QueryInformation> repository = new Dictionary<string, QueryInformation>();
        //List<(string text, string key, string query)> repository = new List<(string, string, string)>();

        public QueryRepository(IUserInteractor userInteractor)
        {
            FillRepository(userInteractor);
        }

        private Func<SqlParameter[]> FuncZerroParameters = () => new SqlParameter[0];
        private void FillRepository(IUserInteractor userInteractor)
        {
            repository.Add("groups", new QueryInformation("If you want to select all groups, write 'groups'",
                CommandExecutionWay.executeReader,
                CommandType.Text,
                () =>
                "SELECT g.[Name], g.[AverageScore], c.[Name], s.[Name] " +
            "FROM [dbo].[Group] g " +
            "JOIN [dbo].[Course] c ON c.Id = g.[CourseId] " +
            "JOIN [dbo].[Specialty] s ON s.Id = g.[SpecialtyId]", FuncZerroParameters));
            repository.Add("students", new QueryInformation("If you want to select all students, write 'students'",
                CommandExecutionWay.executeReader,
                CommandType.Text,
                () =>
                "SELECT st.FirstName, st.LastName, g.[Name], st.[AverageScore] " +
            "FROM [dbo].[Student] st " +
            "JOIN [dbo].[Group] g ON g.Id = st.[GroupId]", FuncZerroParameters));
            repository.Add("scores", new QueryInformation("If you want to select all students scores, write 'scores'",
                CommandExecutionWay.executeReader,
                CommandType.Text,
                () =>
                "SELECT st.FirstName, st.LastName, score.[Value] " +
            "FROM [dbo].[Student] st " +
            "JOIN [dbo].[Score] score ON score.StudentId = st.Id", FuncZerroParameters));
            repository.Add("debts", new QueryInformation("If you want to see all students debts, write 'debts'",
                CommandExecutionWay.executeReader,
                CommandType.StoredProcedure,
                () => "[dbo].[ShowDebts]",
                () =>
                new SqlParameter[] { new SqlParameter("@maxFoursCount", userInteractor.ReadParameter("Write the maximum number of fours")),
                new SqlParameter("@maxCountRetakes", userInteractor.ReadParameter("Write the maximum number of retakes"))}));
            repository.Add("some students", new QueryInformation("If you want to see some students, write 'some students'",
                CommandExecutionWay.executeReader,
                CommandType.StoredProcedure,
                () => "[dbo].[ShowSomeStudents]",
                () =>
                new SqlParameter[] { new SqlParameter("@studentsCount", userInteractor.ReadParameter("Write students count")) }));
            repository.Add("clever students", new QueryInformation("If you want to select clever students, write 'clever students'",
                CommandExecutionWay.executeReader,
                CommandType.Text,
                () => "SELECT DISTINCT st.Id AS StudentId, stud.FirstName, stud.LastName, g.[Name] " +
                "FROM( " +
                "SELECT stt.Id, MIN(scoree.[Value]) AS MinimalScore " +
                "FROM [dbo].[Score] scoree " +
                "JOIN [dbo].[Student] stt ON stt.Id = scoree.StudentId " +
                "JOIN [dbo].[Group] gg ON gg.Id = stt.GroupId " +
                "JOIN [dbo].[Subject] subb ON subb.Id = scoree.SubjectId " +
                "JOIN [dbo].[SubjectCourse] subCoo ON subCoo.SubjectId = subb.Id AND gg.CourseId = subCoo.CourseId " +
                "JOIN [dbo].[SubjectSpecialty] subSpecc ON subSpecc.SubjectId = subb.Id AND gg.SpecialtyId = subSpecc.SpecialtyId " +
                "WHERE scoree.CourseId = gg.CourseId " +
                "GROUP BY stt.ID) st " +
                "JOIN (SELECT st.Id, score.[Value] AS [Score], COUNT(score.[Id]) AS [Count] " +
                "FROM [dbo].[Score] score " +
                "JOIN [dbo].[Student] st ON st.Id = score.StudentId " +
                "JOIN [dbo].[Group] g ON g.Id = st.GroupId " +
                "JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId " +
                "JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id AND g.CourseId = subCo.CourseId " +
                "JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId " +
                "WHERE score.CourseId = g.CourseId " +
                "GROUP BY st.Id, score.[Value]) ts ON ts.Id = st.Id " +
                "JOIN [dbo].Student stud ON stud.Id = ts.Id " +
                "JOIN [dbo].[Group] g ON g.Id = stud.GroupId " +
                "WHERE CAST(1 AS BIT) LIKE CAST( CASE WHEN ts.[Count] <= @maxFoursCount AND st.MinimalScore = 4 THEN 1 ELSE 0 END AS BIT) " +
                "OR st.MinimalScore = 5",
                () => new SqlParameter[] { new SqlParameter("@maxFoursCount", Convert.ToInt32(userInteractor.ReadParameter("Write the maximum number of fours"))) }));
            repository.Add("some groups", new QueryInformation("If you want to see some groups, write 'some groups'",
                CommandExecutionWay.executeReader,
                CommandType.Text,
                () => "SELECT TOP (@groupsCount) *" +
                "FROM [dbo].[Group]",
                () => new SqlParameter[] { new SqlParameter("@groupsCount", Convert.ToInt32(userInteractor.ReadParameter("Write groups count"))) }));
            repository.Add("random string", new QueryInformation("If you want to pick random string, write 'random string'",
                CommandExecutionWay.executeNoQuery,
                CommandType.StoredProcedure,
                () => "[dbo].[PickRandomStringg]",
                () => {
                    var @randomString = new SqlParameter("@randomString", userInteractor.ReadParameter("Write the random string"));
                    var @minLength = new SqlParameter("@minLength", Convert.ToInt32(userInteractor.ReadParameter("Write the minimum length")));
                    var @maxLength = new SqlParameter("@maxLength", Convert.ToInt32(userInteractor.ReadParameter("Write the maximum length")));
                    var @chars = new SqlParameter("@chars", userInteractor.ReadParameter("Write the characters that will make up the length"));
                    @randomString.Size = Convert.ToInt32(@maxLength.Value);
                    @randomString.Direction = ParameterDirection.InputOutput;
                    return new SqlParameter[] { @minLength, @maxLength, @chars, @randomString};
                }));
        }
    }
}
