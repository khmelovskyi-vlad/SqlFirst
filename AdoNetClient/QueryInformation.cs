using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AdoNetClient
{
    struct QueryInformation
    {
        public QueryInformation(string suggestion, CommandExecutionWay commandExecutionWay, CommandType commandType,
            Func<string> funcQuery, Func<SqlParameter[]> funcSqlParameters)
        {
            Suggestion = suggestion;
            CommandExecutionWay = commandExecutionWay;
            this.funcQuery = funcQuery;
            this.funcSqlParameters = funcSqlParameters;
            CommandType = commandType;
        }
        public string Suggestion;
        public CommandExecutionWay CommandExecutionWay;
        public CommandType CommandType;
        private Func<SqlParameter[]> funcSqlParameters;
        public SqlParameter[] SqlParameters
        {
            get { return funcSqlParameters.Invoke(); }
        }
        private Func<string> funcQuery;
        public string Query
        {
            get { return funcQuery.Invoke(); }
        }
    }
}
