using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoNetClient
{
    class SQLCommander
    {
        public SQLCommander(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        IUserInteractor userInteractor;
        public async Task RunCommandSession(SqlConnection connection, SelectionMode mode)
        {
            if (mode == SelectionMode.Free)
            {
                var commandInformation = userInteractor.ReadCommandInformation();
                await RunCommand(commandInformation.Sql, connection, commandInformation.CommandExecutionWay, commandInformation.CommandType, null);
            }
            else if (mode == SelectionMode.Predefined)
            {
                var queryInformation = FindQueryInformation();
                await RunCommand(queryInformation.Query, connection, queryInformation.CommandExecutionWay, queryInformation.CommandType, queryInformation.SqlParameters);
            }
        }
        private async Task RunCommand(string sql, SqlConnection connection, CommandExecutionWay commandExecutionWay, CommandType commandType, SqlParameter[] sqlParameters)
        {
            using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
            {
                sqlCommand.CommandTimeout = int.MaxValue;
                await ExecuteCommand(sqlCommand, commandExecutionWay, commandType, sqlParameters);
            }
        }
        private QueryInformation FindQueryInformation()
        {
            QueryRepository queryRepository = new QueryRepository(userInteractor);
            userInteractor.ShowSuggestions(queryRepository.repository);
            return userInteractor.SelectQuery(queryRepository.repository);
        }
        private async Task ExecuteCommand(SqlCommand sqlCommand, CommandExecutionWay commandExecutionWay, CommandType commandType, SqlParameter[] sqlParameters)
        {
            if (commandType == CommandType.Text)
            {
                switch (commandExecutionWay)
                {
                    case CommandExecutionWay.executeNoQuery:
                        await RunExecutionExecuteNonQuery(sqlCommand);
                        break;
                    case CommandExecutionWay.executeReader:
                        await RunExecutionReader(sqlCommand);
                        break;
                    case CommandExecutionWay.executeScalar:
                        await RunExecutionExecuteeScalar(sqlCommand);
                        break;
                }
            }
            else if(commandType == CommandType.StoredProcedure)
            {
                await RunProcedure(sqlCommand, commandExecutionWay, sqlParameters);
            }
        }
        private async Task RunExecutionReader(SqlCommand sqlCommand)
        {
            //var cancellationTokenSource = new CancellationTokenSource();
            //cancellationTokenSource.Cancel();
            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(/*cancellationTokenSource.Token*/))
            {
                await userInteractor.WriteCommandResult(sqlDataReader);
            }
        }
        private async Task RunExecutionExecuteNonQuery(SqlCommand sqlCommand)
        {
            var rowCount = await sqlCommand.ExecuteNonQueryAsync();
            userInteractor.WriteCountAffectedRows(rowCount);
        }
        private async Task RunExecutionExecuteeScalar(SqlCommand sqlCommand)
        {
            var scalar = await sqlCommand.ExecuteScalarAsync();
            userInteractor.WriteScalar(scalar);
        }
        private async Task RunProcedure(SqlCommand sqlCommand, CommandExecutionWay commandExecutionWay, SqlParameter[] sqlParameters)
        {
            //var par = sqlCommand.CreateParameter();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            if (sqlParameters == null)
            {
                sqlParameters = userInteractor.ReadSqlParameters();
            }
            if (sqlParameters != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
            switch (commandExecutionWay)
            {
                case CommandExecutionWay.executeNoQuery:
                    await RunExecutionExecuteNonQuery(sqlCommand);
                    break;
                case CommandExecutionWay.executeReader:
                    await RunExecutionReader(sqlCommand);
                    break;
                case CommandExecutionWay.executeScalar:
                    await RunExecutionExecuteeScalar(sqlCommand);
                    break;
            }
            //SqlParameter par = new SqlParameter("@randomString", "".PadLeft(200));
            //par.Direction = System.Data.ParameterDirection.InputOutput;
            //sqlCommand.Parameters.Add(new SqlParameter("@minLength", Console.ReadLine()));
            //sqlCommand.Parameters.Add(new SqlParameter("@maxLength", Console.ReadLine()));
            //sqlCommand.Parameters.Add(new SqlParameter("@chars", "qwerfdvbhyun"));
            //sqlCommand.Parameters.Add(par);
            //await sqlCommand.ExecuteNonQueryAsync();
            //var s = sqlCommand.Parameters["@randomString"].Value;
            //var ss = sqlCommand.Parameters["@randomString"].SqlValue;
            //var sss = sqlCommand.Parameters["@randomString"].Value.ToString();
            //Console.WriteLine(sqlCommand.Parameters["@randomString"].Value);
            ////using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
            ////{
            ////    userInteractor.WriteCommandResult(sqlDataReader);
            ////}[PickRandomStringg]
        }
    }
}
