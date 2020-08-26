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
                await RunCommand(sqlCommand, dataOutputWay);
            }
        }
        private async Task RunCommand(SqlCommand sqlCommand, DataOutputWays dataOutputWay)
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
                await RunProcedure(sqlCommand);
            }
        }
        private async Task RunProcedure(SqlCommand sqlCommand)
        {
            //var par = sqlCommand.CreateParameter();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            userInteractor.AddParameters(sqlCommand);
            //SqlParameter par = new SqlParameter("@randomString", "".PadLeft(200));
            //par.Direction = System.Data.ParameterDirection.InputOutput;
            //sqlCommand.Parameters.Add(new SqlParameter("@minLength", Console.ReadLine()));
            //sqlCommand.Parameters.Add(new SqlParameter("@maxLength", Console.ReadLine()));
            //sqlCommand.Parameters.Add(new SqlParameter("@chars", "qwerfdvbhyun"));
            //sqlCommand.Parameters.Add(par);
            await sqlCommand.ExecuteNonQueryAsync();
            var s = sqlCommand.Parameters["@randomString"].Value;
            var ss = sqlCommand.Parameters["@randomString"].SqlValue;
            var sss = sqlCommand.Parameters["@randomString"].Value.ToString();
            Console.WriteLine(sqlCommand.Parameters["@randomString"].Value);
            ////using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
            ////{
            ////    userInteractor.WriteCommandResult(sqlDataReader);
            ////}[PickRandomStringg]
        }
    }
}
