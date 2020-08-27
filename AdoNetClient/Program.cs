using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetClient
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            //QueryInformation queryInformation = new QueryInformation("", "", true, DataOutputWays.executeNoQuery,
            //    () =>
            //    {
            //        Console.WriteLine("Write x parameter");
            //        var parameter = Console.ReadLine();
            //        return "SELECT DISTINCT st.Id AS StudentId, stud.FirstName, stud.LastName, g.[Name]" +
            //"FROM(" +
            //"SELECT stt.Id, MIN(scoree.[Value]) AS MinimalScore" +
            //"FROM [dbo].[Score] scoree" +
            //"JOIN [dbo].[Student] stt ON stt.Id = scoree.StudentId" +
            //"JOIN [dbo].[Group] gg ON gg.Id = stt.GroupId" +
            //"JOIN [dbo].[Subject] subb ON subb.Id = scoree.SubjectId" +
            //"JOIN [dbo].[SubjectCourse] subCoo ON subCoo.SubjectId = subb.Id AND gg.CourseId = subCoo.CourseId" +
            //"JOIN [dbo].[SubjectSpecialty] subSpecc ON subSpecc.SubjectId = subb.Id AND gg.SpecialtyId = subSpecc.SpecialtyId" +
            //"WHERE scoree.CourseId = gg.CourseId" +
            //"GROUP BY stt.ID) st" +
            //"JOIN (SELECT st.Id, score.[Value] AS [Score], COUNT(score.[Id]) AS [Count]" +
            //"FROM [dbo].[Score] score" +
            //"JOIN [dbo].[Student] st ON st.Id = score.StudentId" +
            //"JOIN [dbo].[Group] g ON g.Id = st.GroupId" +
            //"JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId" +
            //"JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id AND g.CourseId = subCo.CourseId" +
            //"JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId" +
            //"WHERE score.CourseId = g.CourseId" +
            //"GROUP BY st.Id, score.[Value]) ts ON ts.Id = st.Id" +
            //"JOIN [dbo].Student stud ON stud.Id = ts.Id" +
            //"JOIN [dbo].[Group] g ON g.Id = stud.GroupId" +
            //$"WHERE CAST(1 AS BIT) LIKE CAST( CASE WHEN ts.[Count] <= {parameter} AND st.MinimalScore = 4 THEN 1 ELSE 0 END AS BIT) " +
            //"OR st.MinimalScore = 5";
            //    });
            //var f = "".GetType().Name;
            //var f2 = 1.GetType().Name;
            //var f3 = 2.35.GetType().Name;
            //var f4 = 44444444444444444.GetType().Name;
            //var f5 = ' '.GetType().Name;
            //Console.WriteLine(f);
            //Console.WriteLine(f2);
            //Console.WriteLine(f3);
            //Console.WriteLine(f4);
            //Console.WriteLine(f5);
            //var query = queryInformation.Query;
            SQLConnector sqlConnector = new SQLConnector(new ConsoleUserInteractor());
            await sqlConnector.RunSqlCli();
            return 1;
        }
        //private static async Task RunSqlCli()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            await RunCommandSession();
        //        }
        //        catch (SqlException ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            Console.WriteLine("If you want to try again, click 'Enter'");
        //            var key = Console.ReadKey();
        //            if (key.Key != ConsoleKey.Enter)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

        //private static async Task RunCommandSession()
        //{
        //    SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
        //    using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        //    {
        //        connection.Open();
        //        Console.WriteLine($"You connected to data base - {connection.Database}");
        //        Console.WriteLine("Enter commant");
        //        var sql = ReadCommandString();
        //        using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
        //        {
        //            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
        //            {
        //                WriteCommandResult(sqlDataReader);
        //            }
        //        }
        //    }
        //}

        //private static void WriteCommandResult(SqlDataReader sqlDataReader)
        //{
        //    while (sqlDataReader.Read())
        //    {
        //        for (int i = 0; i < sqlDataReader.VisibleFieldCount; i++)
        //        {
        //            Console.Write($"{sqlDataReader.GetSqlValue(i),-10}");
        //            if (i < sqlDataReader.VisibleFieldCount - 1)
        //            {
        //                Console.Write(" | ");
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //}

        //private static SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        //{
        //    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
        //    sqlConnectionStringBuilder.DataSource = "WIN-DHV0BQSLTCR";
        //    sqlConnectionStringBuilder.UserID = "SQLFirst";
        //    sqlConnectionStringBuilder.Password = "Test1234";
        //    sqlConnectionStringBuilder.InitialCatalog = "Vlad";
        //    return sqlConnectionStringBuilder;
        //}
        //private static string ReadCommandString()
        //{
        //    var sql = new StringBuilder();
        //    while (true)
        //    {
        //        var key = Console.ReadKey(true);
        //        if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.Enter)
        //        {
        //            Console.WriteLine();
        //            return sql.ToString();
        //        }
        //        else if (key.Modifiers == ConsoleModifiers.Alt && key.Key == ConsoleKey.Enter)
        //        {
        //            Console.WriteLine();
        //            return sql.ToString();
        //        }
        //        else if (key.Key == ConsoleKey.Enter)
        //        {
        //            Console.WriteLine();
        //            sql.Append(key.KeyChar);
        //        }
        //        else if (key.Key == ConsoleKey.Backspace)
        //        {
        //            sql.Remove(sql.Length - 1, 1);
        //            Console.Write("\b \b");
        //        }
        //        else
        //        {
        //            Console.Write(key.KeyChar);
        //            sql.Append(key.KeyChar);
        //        }
        //    }
        //}
    }
}
