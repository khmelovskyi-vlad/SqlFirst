using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetClient
{
    class SQLCommander
    {
        public SQLCommander(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
            sqlWriter = new SQLWriter(userInteractor);
        }
        IUserInteractor userInteractor;
        SQLWriter sqlWriter;

        public async Task RunCommandSession(SqlConnection connection)
        {
            Console.WriteLine("Enter commant");
            var (sql, dataOutputWay) = userInteractor.ReadCommandInformation();
            using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
            {
                if (dataOutputWay == DataOutputWays.executeReader)
                {
                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        await userInteractor.WriteCommandResult(sqlDataReader);
                    }
                }
                else if (dataOutputWay == DataOutputWays.executeNoQuery)
                {
                    var rowCount = await sqlCommand.ExecuteNonQueryAsync();
                    userInteractor.WriteCountAffectedRows(rowCount);
                }
                else if (dataOutputWay == DataOutputWays.executeScalar)
                {
                    var scalar = await sqlCommand.ExecuteScalarAsync();
                    userInteractor.WriteScalar(scalar);
                }
                else if (dataOutputWay == DataOutputWays.executeProcedure)
                {
                    //var par = sqlCommand.CreateParameter();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //userInteractor.AddParameters(sqlCommand);
                    sqlCommand.Parameters.Add(new SqlParameter("@minLength", 7));
                    sqlCommand.Parameters.Add(new SqlParameter("@maxLength", 50));
                    sqlCommand.Parameters.Add(new SqlParameter("@chars", "qwerfdvbhyun"));
                    sqlCommand.Parameters.Add(new SqlParameter("@randomString", ""));
                    sqlCommand.Parameters["@randomString"].Direction = System.Data.ParameterDirection.InputOutput;
                    await sqlCommand.ExecuteNonQueryAsync();
                    var s = sqlCommand.Parameters["@randomString"].Value;
                    var ss = sqlCommand.Parameters["@randomString"].Value.ToString();
                    Console.WriteLine(sqlCommand.Parameters["@randomString"].Value);
                    //using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    //{
                    //    userInteractor.WriteCommandResult(sqlDataReader);
                    //}[PickRandomStringg]
                }
            }
        }
    }
}
