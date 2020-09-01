using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class DataSetMaster
    {
        public DataSetMaster(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        private IUserInteractor userInteractor;
        DataSet dataSet = new DataSet();
        public void Run()
        {
            if (userInteractor.ReadCreationMode() == CreationMode.Predefined)
            {
                dataSet = userInteractor.SelectDataSet();
                //foreach (var row in dataSet.Tables["Student"].Select())
                //{
                //    foreach (var item in row.ItemArray)
                //    {
                //        Console.Write($"{item , 20} |");
                //    }
                //    Console.WriteLine();
                //}
            }
            else
            {
                var tables = userInteractor.CreateDataSetTables();
                dataSet.Tables.AddRange(tables);
            }
        }
    }
}
