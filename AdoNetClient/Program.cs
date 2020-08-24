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
