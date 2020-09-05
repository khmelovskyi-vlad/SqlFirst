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
            var cancellationTokenSource = new CancellationTokenSource();
            Task task;
            if (mode == SelectionMode.Free)
            {
                var commandInformation = userInteractor.ReadCommandInformation();
                task = RunCommand(commandInformation.Sql, connection, commandInformation.CommandExecutionWay, commandInformation.CommandType,
                    userInteractor.ReadSqlParameters(), cancellationTokenSource.Token);
            }
            else/* if (mode == SelectionMode.Predefined)*/
            {
                var queryInformation = FindQueryInformation();
                task = RunCommand(queryInformation.Query, connection, queryInformation.CommandExecutionWay, queryInformation.CommandType, queryInformation.SqlParameters,
                    cancellationTokenSource.Token);
            }
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
            await userInteractor.SelectContinuation(cancellationTokenSource, task);
        }
        private QueryInformation FindQueryInformation()
        {
            QueryRepository queryRepository = new QueryRepository(userInteractor);
            userInteractor.ShowSuggestions(queryRepository.repository);
            return userInteractor.SelectQuery(queryRepository.repository);
        }
        private async Task RunCommand(string sql, SqlConnection connection, CommandExecutionWay commandExecutionWay, CommandType commandType, 
            SqlParameter[] sqlParameters, CancellationToken cancellationToken)
        {
            using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
            {
                sqlCommand.CommandTimeout = int.MaxValue;
                await ExecuteCommand(sqlCommand, commandExecutionWay, commandType, sqlParameters, cancellationToken);
            }
        }
        private async Task ExecuteCommand(SqlCommand sqlCommand, CommandExecutionWay commandExecutionWay, CommandType commandType, 
            SqlParameter[] sqlParameters, CancellationToken cancellationToken)
        {
            sqlCommand.CommandType = commandType;
            AddParameters(sqlParameters, sqlCommand);
            if (commandType == CommandType.Text)
            {
                switch (commandExecutionWay)
                {
                    case CommandExecutionWay.executeNoQuery:
                        await RunExecutionExecuteNonQuery(sqlCommand, cancellationToken);
                        break;
                    case CommandExecutionWay.executeReader:
                        await RunExecutionReader(sqlCommand, cancellationToken);
                        break;
                    case CommandExecutionWay.executeScalar:
                        await RunExecutionExecuteeScalar(sqlCommand, cancellationToken);
                        break;
                }
            }
            else if(commandType == CommandType.StoredProcedure)
            {
                await RunProcedure(sqlCommand, commandExecutionWay, sqlParameters, cancellationToken);
            }
        }
        private async Task RunExecutionReader(SqlCommand sqlCommand, CancellationToken cancellationToken)
        {
            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken))
            {
                await userInteractor.WriteCommandResult(sqlDataReader, cancellationToken);
            }
        }
        private async Task RunExecutionExecuteNonQuery(SqlCommand sqlCommand, CancellationToken cancellationToken)
        {
            var rowCount = await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
            userInteractor.WriteCountAffectedRows(rowCount);
        }
        private async Task RunExecutionExecuteeScalar(SqlCommand sqlCommand, CancellationToken cancellationToken)
        {
            var scalar = await sqlCommand.ExecuteScalarAsync(cancellationToken);
            userInteractor.WriteScalar(scalar);
        }
        private void AddParameters(SqlParameter[] sqlParameters, SqlCommand sqlCommand)
        {
            if (sqlParameters != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }
        }
        private async Task RunProcedure(SqlCommand sqlCommand, CommandExecutionWay commandExecutionWay, SqlParameter[] sqlParameters, CancellationToken cancellationToken)
        {
            switch (commandExecutionWay)
            {
                case CommandExecutionWay.executeNoQuery:
                    await RunExecutionExecuteNonQuery(sqlCommand, cancellationToken);
                    break;
                case CommandExecutionWay.executeReader:
                    await RunExecutionReader(sqlCommand, cancellationToken);
                    break;
                case CommandExecutionWay.executeScalar:
                    await RunExecutionExecuteeScalar(sqlCommand, cancellationToken);
                    break;
            }
            userInteractor.WriteParametersResult(sqlCommand.Parameters);
        }
    }
}
