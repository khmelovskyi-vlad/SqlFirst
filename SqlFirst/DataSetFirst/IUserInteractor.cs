using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    interface IUserInteractor
    {
        DataTable[] CreateDataSetTables();
        CreationMode ReadCreationMode();
        DataSet SelectDataSet();
        bool CheckNeedAddToDataBase(string message);
    }
}
