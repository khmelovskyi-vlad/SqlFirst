using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class DataSetPrototype
    {
        public DataSetPrototype(DataSet dataSet, string suggestion)
        {
            DataSet = dataSet;
            Suggestion = suggestion;
        }
        public DataSet DataSet;
        public string Suggestion;
    }
}
