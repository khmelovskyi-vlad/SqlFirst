using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace AdoNetClient
{
    struct QueryInformation
    {
        public QueryInformation(/*string key, */string suggestion, DataOutputWays outputWay, Func<string> funcQuery, Func<ProcedureInformation> funcProcedureInformation)
        {
            //Key = key;
            Suggestion = suggestion;
            OutputWay = outputWay;
            //Query = funcQuery.Invoke();
            this.funcQuery = funcQuery;
            this.funcProcedureInformation = funcProcedureInformation;
        }
        private Func<ProcedureInformation> funcProcedureInformation;
        public ProcedureInformation ProcedureInformation
        {
            get { return funcProcedureInformation.Invoke(); }
        }
        private Func<string> funcQuery;
        public string Query
        {
            get { return funcQuery.Invoke(); }
        }
        //public string Key;
        public string Suggestion;
        public DataOutputWays OutputWay;
    }
}
