using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetClient
{
    class ConsoleUserInteractor : IUserInteractor
    {
        //public void Write(string value)
        //{
        //    Console.Write(value);
        //}

        //public void Write(char value)
        //{
        //    Console.Write(value);
        //}

        //public void WriteLine(string value)
        //{
        //    Console.WriteLine(value);
        //}

        //public void WriteLine()
        //{
        //    Console.WriteLine();
        //}
        public CommandPrototype ReadCommandInformation()
        {
            WriteInstruction();
            List<StringBuilder> stringBuilders = new List<StringBuilder>();
            var stringBuilder = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if ((key.Modifiers & ConsoleModifiers.Control) != 0 && (key.Modifiers & ConsoleModifiers.Shift) != 0 && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (new CommandPrototype(CreateSql(stringBuilder, stringBuilders), CommandExecutionWay.executeScalar, System.Data.CommandType.Text));
                }
                else if(key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (new CommandPrototype(CreateSql(stringBuilder, stringBuilders), CommandExecutionWay.executeReader, System.Data.CommandType.Text));
                }
                else if ((key.Modifiers & ConsoleModifiers.Shift) != 0 && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (new CommandPrototype(CreateSql(stringBuilder, stringBuilders), CommandExecutionWay.executeNoQuery, System.Data.CommandType.Text));
                }
                else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key == ConsoleKey.P)
                {
                    Console.WriteLine();
                    return (new CommandPrototype(CreateSql(stringBuilder, stringBuilders), SelectProcedureOutputWay(), System.Data.CommandType.StoredProcedure));
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    stringBuilder.Append(key.KeyChar);
                    stringBuilders.Add(stringBuilder);
                    stringBuilder = new StringBuilder();
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    stringBuilder = StartBackspaceChanges(stringBuilder, stringBuilders);
                }
                else
                {
                    Console.Write(key.KeyChar);
                    stringBuilder.Append(key.KeyChar);
                }
            }
        }
        private void WriteInstruction()
        {
            Console.WriteLine("If you want to execute scalar, press 'Control + Shift + Enter' at the end");
            Console.WriteLine("If you want to execute reader, press 'Control + Enter' at the end");
            Console.WriteLine("If you want to execute no query, press 'Shift + Enter' at the end");
            Console.WriteLine("If you want to execute a procedure, press 'Control + P' at the end");
            Console.WriteLine("Enter commant");
        }
        private CommandExecutionWay SelectProcedureOutputWay()
        {
            Console.WriteLine("If this procedure output some information, click 'i'");
            Console.WriteLine("If this procedure output a scalar, click 's'");
            Console.WriteLine("If this procedure does not output anything, click something else");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.I)
            {
                return CommandExecutionWay.executeReader;
            }
            else if (key.Key == ConsoleKey.S)
            {
                return CommandExecutionWay.executeScalar;
            }
            else
            {
                return CommandExecutionWay.executeNoQuery;
            }
        }
        //private bool CheckIsProcedure()
        //{
        //    Console.WriteLine("If this is a procedure, click 'p'");
        //    var key = Console.ReadKey();
        //    if (key.Key == ConsoleKey.P)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        private string CreateSql(StringBuilder stringBuilder, List<StringBuilder> stringBuilders)
        {
            stringBuilders.Add(stringBuilder);
            var sql = "";
            foreach (var newStringBuilder in stringBuilders)
            {
                sql += newStringBuilder.ToString();
            }
            return sql;
        }
        private StringBuilder StartBackspaceChanges(StringBuilder stringBuilder, List<StringBuilder> stringBuilders)
        {
            if (stringBuilder.Length > 0 || stringBuilders.Count > 0)
            {
                if (stringBuilder.Length == 0)
                {
                    if (stringBuilders.Count > 0)
                    {
                        stringBuilder = stringBuilders.Last();
                        stringBuilders.RemoveAt(stringBuilders.Count() - 1);
                        Console.CursorTop = Console.CursorTop - 1;
                        Console.CursorLeft = stringBuilder.Length - 1;
                    }
                }
                else
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            return stringBuilder;
        }
        public async Task WriteCommandResult(SqlDataReader sqlDataReader)
        {
            var columns = sqlDataReader.GetColumnSchema();
            var typeWriteData = FindTypeWriteData(columns);
            WriteColumns(columns, typeWriteData);
            await WriteData(sqlDataReader, typeWriteData);
        }
        private async Task WriteData(SqlDataReader sqlDataReader, WriteTypeData typeWriteData)
        {
            if (typeWriteData == WriteTypeData.withBigWindow)
            {
                await WriteDataWithBigWindow(sqlDataReader);
            }
            else if (typeWriteData == WriteTypeData.withSmallWindow)
            {
                await WriteDataWithSmallWindow(sqlDataReader);
            }
        }
        private async Task WriteDataWithSmallWindow(SqlDataReader sqlDataReader)
        {
            while (await sqlDataReader.ReadAsync())
            {
                for (int i = 0; i < sqlDataReader.VisibleFieldCount; i++)
                {
                    Console.Write($"{await sqlDataReader.GetFieldValueAsync<object>(i),-20}");
                    if (i < sqlDataReader.VisibleFieldCount - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }
        private void WriteColumns(ReadOnlyCollection<DbColumn> columns, WriteTypeData typeWriteData)
        {
            if (typeWriteData == WriteTypeData.withBigWindow)
            {
                WriteColumnsWithBigWindow(columns);
            }
            else if (typeWriteData == WriteTypeData.withSmallWindow)
            {
                WriteColumnsWithSmallWindow(columns);
            }
        }
        private WriteTypeData FindTypeWriteData(ReadOnlyCollection<DbColumn> columns)
        {
            if (Console.BufferWidth / columns.Count - 3 <= 0)
            {
                return WriteTypeData.withSmallWindow;
            }
            else
            {
                return WriteTypeData.withBigWindow;
            }
        }
        private async Task WriteDataWithBigWindow(SqlDataReader sqlDataReader)
        {
            while (await sqlDataReader.ReadAsync())
            {
                var dataNames = new string[sqlDataReader.VisibleFieldCount];
                for (int i = 0; i < dataNames.Length; i++)
                {
                    dataNames[i] = (await sqlDataReader.GetFieldValueAsync<object>(i)).ToString();
                }
                WriteColumns(dataNames);
                Console.WriteLine();
            }
        }
        private void WriteColumnsWithSmallWindow(ReadOnlyCollection<DbColumn> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                Console.Write($"{columns[i].ColumnName,-20}");
                if (i < columns.Count - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
        private void WriteColumnsWithBigWindow(ReadOnlyCollection<DbColumn> columns)
        {
            var columnsNames = columns.Select(column => column.ColumnName).ToArray();
            WriteColumns(columnsNames);
            Console.WriteLine();
        }
        private void WriteColumns(string[] names)
        {
            var cursorTop = Console.CursorTop;
            var countInOneSegment = Console.BufferWidth / names.Length;
            for (int i = 0; i < names.Length; i++)
            {
                var startIndex = countInOneSegment * i;
                if (i < names.Length - 1)
                {
                    WriteDataName(countInOneSegment - 3, startIndex, cursorTop, names[i]);
                }
                else
                {
                    WriteDataName(countInOneSegment, startIndex, cursorTop, names[i]);
                }
            }
        }
        private void WriteDataName(int weigh, int startIndex, int top, string name)
        {
            if (name.Length + 3 > weigh)
            {
                WriteBigData(name, weigh, startIndex, top);
            }
            else
            {
                WriteSmallData(name, startIndex, top);
            }
        }
        private void WriteSmallData(string data, int startIndex, int top)
        {
            Console.SetCursorPosition(startIndex, top);
            Console.Write(data);
        }
        private void WriteBigData(string data, int weidth, int startIndex, int top)
        {
            var countInOneSegment = weidth;
            var leftCursor = startIndex;
            for (int i = 0; i < data.Length; i++)
            {
                if (i % countInOneSegment == 0 && i != 0)
                {
                    top++;
                    leftCursor = startIndex;
                }
                Console.SetCursorPosition(leftCursor, top);
                Console.Write(data[i]);
                leftCursor++;
            }
        }
        public void WriteCountAffectedRows(int rowsCount)
        {
            Console.WriteLine($"{rowsCount} row(s) affected");
        }

        public void WriteScalar(object value)
        {
            Console.WriteLine(value);
        }
        public SqlParameter[] ReadSqlParameters()
        {
            return ReadParameters();
        }
        private SqlParameter[] ReadParameters()
        {
            var parametersCount = ReadParametersCount();
            var sqlParameters = new SqlParameter[parametersCount];
            for (int i = 0; i < parametersCount; i++)
            {
                var sqlParameter = CreateSqlParameterWithDirection();
                sqlParameter.ParameterName = ReadParameterName();
                Console.WriteLine("Write parameter value");
                sqlParameter.Value = Console.ReadLine();
                sqlParameters[i] = sqlParameter;
            }
            return sqlParameters;
        }
        private string ReadParameterName()
        {
            Console.WriteLine("Write parameter name");
            return Console.ReadLine();
        }
        //private Type ReadParameterType()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("If you want to choose the int type, click 'i'");
        //        Console.WriteLine("If you want to choose the long type, click 'l'");
        //        Console.WriteLine("If you want to choose the double type, click 'd'");
        //        Console.WriteLine("If you want to choose the char type, click 'c'");
        //        Console.WriteLine("If you want to choose the string type, click 's'");
        //        var key = Console.ReadKey();
        //        switch (key.Key)
        //        {int
        //            case ConsoleKey.I:
        //                return 1.GetType();
        //            default:
        //        }
        //    }
        //}
        //private SqlParameter SetParameterType(Type type)
        //{
        //    switch (type.Name)
        //    {
        //        case type.
        //        default:
        //            break;
        //    }
        //}
        private SqlParameter CreateSqlParameterWithDirection()
        {
            var sqlParameter = new SqlParameter();
            Console.WriteLine("Choose parameter direction");
            while (true)
            {
                Console.WriteLine("If you want to choose the Input parameter, click 'i'");
                Console.WriteLine("If you want to choose the InputOutput parameter, click 'o'");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.I:
                        sqlParameter.Direction = System.Data.ParameterDirection.Input;
                        return sqlParameter;
                    case ConsoleKey.O:
                        sqlParameter.Direction = System.Data.ParameterDirection.InputOutput;
                        return sqlParameter;
                    default:
                        Console.WriteLine("Bed input, try again");
                        break;
                }
            }
        }
        private int ReadParametersCount()
        {
            while (true)
            {
                Console.WriteLine("Wrire parameters count");
                var line = Console.ReadLine();
                try
                {
                    var count = Convert.ToInt32(line);
                    if (count >= 0)
                    {
                        return count;
                    }
                    else
                    {
                        Console.WriteLine("Parameters count can't be be less than 0");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bed input, try again");
                }
            }
        }

        public SelectionMode SelectMode()
        {
            while (true)
            {
                Console.WriteLine("If you want to use the predefined query, click 'p'");
                Console.WriteLine("If you want to use the new query, click 'n'");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        return SelectionMode.Predefined;
                    case ConsoleKey.N:
                        return SelectionMode.Free;
                }
                Console.WriteLine("Bed input, try again");
            }
        }

        public void ShowSuggestions(Dictionary<string, QueryInformation> repository)
        {
            foreach (var data in repository)
            {
                Console.WriteLine(data.Value.Suggestion);
            }
        }
        public QueryInformation SelectQuery(Dictionary<string, QueryInformation> repository)
        {
            while (true)
            {
                var line = Console.ReadLine();
                foreach (var data in repository)
                {
                    if (data.Key == line)
                    {
                        return data.Value;
                    }
                }
                Console.WriteLine("Bed input, try again");
            }
        }


        public string ReadParameter(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public void WriteExceptionMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
