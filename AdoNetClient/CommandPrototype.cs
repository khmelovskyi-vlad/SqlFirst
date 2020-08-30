using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoNetClient
{
    class CommandPrototype
    {
        public CommandPrototype(string sql, CommandExecutionWay commandExecutionWay, CommandType commandType)
        {
            Sql = sql;
            CommandExecutionWay = commandExecutionWay;
            CommandType = commandType;
        }
        public string Sql;
        public CommandExecutionWay CommandExecutionWay;
        public CommandType CommandType;
    }
}
