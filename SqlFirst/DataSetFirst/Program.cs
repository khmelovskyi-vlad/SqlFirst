using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSetMaster dataSetMaster = new DataSetMaster(new ConsoleUserInteractor());
            dataSetMaster.Run();
            DataSet dataSet = new DataSet();
            var studentTable = dataSet.Tables.Add("Student");
            var id = studentTable.Columns.Add("Id", typeof(Guid));
            studentTable.Columns.Add("FirstName", typeof(string));
            studentTable.Columns.Add("LastName", typeof(string));
            studentTable.Columns.Add("GroupId", typeof(Guid));
            studentTable.Columns.Add("AverageScore", typeof(Int32));
            studentTable.PrimaryKey = new DataColumn[] { id };
            SqlConnectionStringBuilder sqlConnectionStringBuilder = GetSqlConnectionStringBuilder();
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                //sqlDataAdapter.TableMappings.Add("Table", "Suppliers");
                sqlDataAdapter.SelectCommand = new SqlCommand("SELECT TOP 10 *" +
                    "FROM [dbo].[Student]", sqlConnection);
                sqlDataAdapter.Fill(studentTable);
                //sqlDataAdapter.Update
                //dataSet.Merge();
                //dataSet.Container
            }
            var rows = studentTable.Select();
            foreach (var row in rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }
            Console.ReadKey();
        }
        private static SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
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
