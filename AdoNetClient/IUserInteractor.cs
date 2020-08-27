using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetClient
{
    interface IUserInteractor
    {
        //void Write(string value);
        //void Write(char value);
        //void WriteLine(string value);
        //void WriteLine();
        SelectionModes SelectMode();
        (string, DataOutputWays) ReadCommandInformation();
        Task WriteCommandResult(SqlDataReader sqlDataReader);
        void WriteCountAffectedRows(int rowsCount);
        void WriteScalar(object value);
        void AddParameters(SqlCommand sqlCommand);
        void ShowSuggestions(Dictionary<string, QueryInformation> repository);
        QueryInformation SelectQuery(Dictionary<string, QueryInformation> repository);
        DataOutputWays SelectDataOutputWay();
        string ReadParameter(string message);
        void WriteExceptionMessage(SqlException ex);
    }
}
