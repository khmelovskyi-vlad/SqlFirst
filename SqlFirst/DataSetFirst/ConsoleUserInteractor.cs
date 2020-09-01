using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetFirst
{
    class ConsoleUserInteractor : IUserInteractor
    {
        public DataTable[] CreateDataSetTables()
        {
            List<DataTable> dataTables = new List<DataTable>();
            while (true)
            {
                try
                {
                    DataTable dataTable = new DataTable(ReadSomeName("Write the table name"));
                    if (!dataTables.Select(table => table.TableName).Contains(dataTable.TableName))
                    {
                        var columns = CreateColumns();
                        dataTable.Columns.AddRange(columns);
                        dataTable.PrimaryKey = ReadPrimaryKey(columns);
                        dataTables.Add(dataTable);
                        if (!CheckNeedContinuetion("If you want to create more tables, click 'Enter'", ConsoleKey.Enter))
                        {
                            return dataTables.ToArray();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Have this table, write else");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Bad input, try again");
                }
            }
            //var student = dataSet.Tables.Add("Student");
            //var id = student.Columns.Add("Id", typeof(Guid));
            //student.Columns.Add("FirstName", typeof(string));
            //student.Columns.Add("LastName", typeof(string));
            //student.Columns.Add("GroupId", typeof(Guid));
            //student.Columns.Add("AverageScore", typeof(Int32));
            //student.PrimaryKey = new DataColumn[] { id };
        }
        //public DataRelation CreateDataRelation()
        //{

        //}
        private DataColumn[] ReadPrimaryKey(DataColumn[] columns)
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            if (CheckNeedContinuetion("If you want to add the primary key, click 'Enter'", ConsoleKey.Enter))
            {
                while (true)
                {
                    var columnName = ReadSomeName("Write the column name");
                    var columnsWithReadName = columns.Where(column => column.ColumnName == columnName);
                    if (columnsWithReadName.Count() == 1)
                    {
                        dataColumns.Add(columnsWithReadName.First());
                    }
                    else
                    {
                        Console.WriteLine("Don't have this column");
                    }
                    if (!CheckNeedContinuetion("If you want to add more columns to primary key, click 'Enter'", ConsoleKey.Enter))
                    {
                        break;
                    }
                }
            }
            return dataColumns.ToArray();
        }
        private bool CheckNeedContinuetion(string message, ConsoleKey consoleKey)
        {
            Console.WriteLine(message);
            if (Console.ReadKey(true).Key != consoleKey)
            {
                return false;
            }
            return true;
        }
        private string ReadSomeName(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
        private DataColumn[] CreateColumns()
        {
            List<DataColumn> dataColumns = new List<DataColumn>();
            while (true)
            {
                DataColumn dataColumn = new DataColumn(ReadSomeName("Write the column name"), ReadColumnType());
                if (!dataColumns.Select(column => column.ColumnName).Contains(dataColumn.ColumnName))
                {
                    dataColumns.Add(dataColumn);
                    if (!CheckNeedContinuetion("If you want to create more columns, click 'Enter'", ConsoleKey.Enter))
                    {
                        return dataColumns.ToArray();
                    }
                }
                else
                {
                    Console.WriteLine("Have this column, write else");
                }
            }
        }
        private Type ReadColumnType()
        {
            while (true)
            {
                try
                {
                    //System.String
                    //System.Int32
                    //System.Guid
                    Console.WriteLine("Write a need type");
                    var typeName = Console.ReadLine();
                    var type = Type.GetType(typeName);
                    if (type == null)
                    {
                        Console.WriteLine("Bad input, try again");
                        continue;
                    }
                    return type;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Bad input, try again");
                }
            }
        }
        //private void WriteExceptionMessage(Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    Console.WriteLine("Bad input, try again");
        //}
        public CreationMode ReadCreationMode()
        {
            Console.WriteLine("If you want to use the saved data set, click 'Enter'");
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return CreationMode.Predefined;
            }
            else
            {
                return CreationMode.Free;
            }
        }

        public DataSet SelectDataSet()
        {
            DataSetRepository dataSetRepository = new DataSetRepository();
            WriteSuggestions(dataSetRepository.Repository);
            return ReadDataSet(dataSetRepository.Repository);
        }
        private DataSet ReadDataSet(Dictionary<string, DataSetPrototype> repository)
        {
            while (true)
            {
                var line = Console.ReadLine();
                foreach (var item in repository)
                {
                    if (item.Key == line)
                    {
                        return item.Value.DataSet;
                    }
                }
                Console.WriteLine("Don't have this data set, write else");
            }
        }
        private void WriteSuggestions(Dictionary<string, DataSetPrototype> repository)
        {
            foreach (var item in repository)
            {
                Console.WriteLine(item.Value.Suggestion);
            }
        }
    }
}
