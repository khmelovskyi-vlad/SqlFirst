using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetClient
{
    class SQLConnector
    {
        public SQLConnector(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        IUserInteractor userInteractor;
        public async Task RunSqlCli()
        {
            while (true)
            {
                try
                {
                    await ConnectClient();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("If you want to try again, click 'Enter'");
                    var key = Console.ReadKey();
                    if (key.Key != ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            }
        }
        private async Task ConnectClient()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
            using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                Console.WriteLine($"You connected to data base - {connection.Database}");
                SQLCommander sqlCommander = new SQLCommander(userInteractor);
                await sqlCommander.RunCommandSession(connection);
            }
        }
        private SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = "WIN-DHV0BQSLTCR";
            sqlConnectionStringBuilder.UserID = "SQLFirst";
            sqlConnectionStringBuilder.Password = "Test1234";
            sqlConnectionStringBuilder.InitialCatalog = "Vlad";
            return sqlConnectionStringBuilder;
        }
    }
}
