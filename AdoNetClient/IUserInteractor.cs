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
        (string, DataOutputWays) ReadCommandInformation();
        Task WriteCommandResult(SqlDataReader sqlDataReader);
        void WriteCountAffectedRows(int rowsCount);
        void WriteScalar(object value);
        void AddParameters(SqlCommand sqlCommand);
    }
}
