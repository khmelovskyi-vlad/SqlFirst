using System;
using System.Collections.Generic;
using System.Text;

namespace AdoNetClient
{
    struct QueryInformation
    {
        public QueryInformation(/*string key, */string suggestion, DataOutputWays outputWay, Func<string> funcQuery)
        {
            //Key = key;
            Suggestion = suggestion;
            OutputWay = outputWay;
            //Query = funcQuery.Invoke();
            this.funcQuery = funcQuery;
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
