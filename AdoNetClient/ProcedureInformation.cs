using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AdoNetClient
{
    class ProcedureInformation
    {
        public ProcedureInformation(DataOutputWays outputWay, SqlParameter[] parameters)
        {
            OutputWay = outputWay;
            Parameters = parameters;
        }
        public DataOutputWays OutputWay;
        public SqlParameter[] Parameters;
    }
}
