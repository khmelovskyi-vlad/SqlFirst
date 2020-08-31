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
            await sqlConnector.RunSqlClient();
            return 1;
        }
    }
}
