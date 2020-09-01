using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class SqlCommander
    {
        public void Run(SqlConnectionStringBuilder sqlConnectionStringBuilder, Action<SqlConnection> action)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                action(sqlConnection);
            }
        }
    }
}
