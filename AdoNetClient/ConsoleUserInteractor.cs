using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
                        stringBuilder.Remove(stringBuilder.Length - 1, 1);
                        stringBuilders.RemoveAt(stringBuilders.Count() - 1);
                        Console.CursorTop = Console.CursorTop - 1;
                        Console.CursorLeft = stringBuilder.Length;
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
        public async Task WriteCommandResult(SqlDataReader sqlDataReader, CancellationToken cancellationTokenSource)
        {
            var columns = sqlDataReader.GetColumnSchema();
            var typeWriteData = FindTypeWriteData(columns);
            WriteColumns(columns, typeWriteData);
            await WriteData(sqlDataReader, typeWriteData, cancellationTokenSource);
        }
        private async Task WriteData(SqlDataReader sqlDataReader, WriteTypeData typeWriteData, CancellationToken cancellationTokenSource)
        {
            if (typeWriteData == WriteTypeData.withBigWindow)
            {
                await WriteDataWithBigWindow(sqlDataReader, cancellationTokenSource);
            }
            else if (typeWriteData == WriteTypeData.withSmallWindow)
            {
                await WriteDataWithSmallWindow(sqlDataReader, cancellationTokenSource);
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
        private async Task WriteDataWithSmallWindow(SqlDataReader sqlDataReader, CancellationToken cancellationTokenSource)
        {
            var readTask = sqlDataReader.ReadAsync(cancellationTokenSource);
            if (readTask.IsCanceled)
            {
                return;
            }
            while (await readTask)
            {
                for (int i = 0; i < sqlDataReader.VisibleFieldCount; i++)
                {
                    var task = sqlDataReader.GetFieldValueAsync<object>(i, cancellationTokenSource);
                    if (task.IsCanceled)
                    {
                        return;
                    }
                    Console.Write($"{await task,-20}");
                    if (i < sqlDataReader.VisibleFieldCount - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }
        private async Task WriteDataWithBigWindow(SqlDataReader sqlDataReader, CancellationToken cancellationTokenSource)
        {
            var readTask = sqlDataReader.ReadAsync(cancellationTokenSource);
            if (readTask.IsCanceled)
            {
                return;
            }
            while (await readTask)
            {
                var dataNames = new string[sqlDataReader.VisibleFieldCount];
                for (int i = 0; i < dataNames.Length; i++)
                {
                    var task = sqlDataReader.GetFieldValueAsync<object>(i, cancellationTokenSource);
                    if (task.IsCanceled)
                    {
                        return;
                    }
                    dataNames[i] = (await task).ToString();
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
                if (СheckNeed("If you want to select the type, click 'Enter'"))
                {
                    sqlParameter.SqlDbType = ReadType();
                }
                if (СheckNeed("If you want to select the size, click 'Enter'"))
                {
                    sqlParameter.Size = ReadParameterSize();
                }
                sqlParameters[i] = sqlParameter;
            }
            return sqlParameters;
        }
        private int ReadParameterSize()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Write the size");
                    var line = Console.ReadLine();
                    var size = Convert.ToInt32(line);
                    if (size >= 0)
                    {
                        return size;
                    }
                    else
                    {
                        Console.WriteLine("The size can't be less than 0");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Bad input, try again");
                }

            }
        }
        private bool СheckNeed(string message)
        {
            Console.WriteLine(message);
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return true;
            }
            return false;
        }
        private void WriteInstructionForFindType()
        {
            Console.WriteLine("If you want to select the BigInt type, click 'A'");
            Console.WriteLine("If you want to select the Binary type, click 'B'");
            Console.WriteLine("If you want to select the Bit type, click 'C'");
            Console.WriteLine("If you want to select the Char type, click 'D'");
            Console.WriteLine("If you want to select the DateTime type, click 'E'");
            Console.WriteLine("If you want to select the Decimal type, click 'F'");
            Console.WriteLine("If you want to select the Float type, click 'G'");
            Console.WriteLine("If you want to select the Image type, click 'H'");
            Console.WriteLine("If you want to select the Int type, click 'I'");
            Console.WriteLine("If you want to select the Money type, click 'J'");
            Console.WriteLine("If you want to select the NChar type, click 'K'");
            Console.WriteLine("If you want to select the NText type, click 'L'");
            Console.WriteLine("If you want to select the NVarChar type, click 'M'");
            Console.WriteLine("If you want to select the Real type, click 'N'");
            Console.WriteLine("If you want to select the UniqueIdentifier type, click 'O'");
            Console.WriteLine("If you want to select the SmallDateTime type, click 'P'");
            Console.WriteLine("If you want to select the SmallInt type, click 'Q'");
            Console.WriteLine("If you want to select the SmallMoney type, click 'R'");
            Console.WriteLine("If you want to select the Text type, click 'S'");
            Console.WriteLine("If you want to select the Timestamp type, click 'T'");
            Console.WriteLine("If you want to select the TinyInt type, click 'U'");
            Console.WriteLine("If you want to select the VarBinary type, click 'V'");
            Console.WriteLine("If you want to select the VarChar type, click 'W'");
            Console.WriteLine("If you want to select the Variant type, click 'X'");
            Console.WriteLine("If you want to select the Xml type, click 'Y'");
            Console.WriteLine("If you want to select the Udt type, click 'Z'");
            Console.WriteLine("If you want to select the Structured type, click 'Enter'");
            Console.WriteLine("If you want to select the Date type, click 'Delete'");
            Console.WriteLine("If you want to select the Time type, click 'Backspace'");
            Console.WriteLine("If you want to select the DateTime2 type, click 'Escape'");
            Console.WriteLine("If you want to select the DateTimeOffset type, click 'Tab'");
        }
        private SqlDbType ReadType()
        {
            WriteInstructionForFindType();
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        return SqlDbType.BigInt;
                    case ConsoleKey.B:
                        return SqlDbType.Binary;
                    case ConsoleKey.C:
                        return SqlDbType.Bit;
                    case ConsoleKey.D:
                        return SqlDbType.Char;
                    case ConsoleKey.E:
                        return SqlDbType.DateTime;
                    case ConsoleKey.F:
                        return SqlDbType.Decimal;
                    case ConsoleKey.G:
                        return SqlDbType.Float;
                    case ConsoleKey.H:
                        return SqlDbType.Image;
                    case ConsoleKey.I:
                        return SqlDbType.Int;
                    case ConsoleKey.J:
                        return SqlDbType.Money;
                    case ConsoleKey.K:
                        return SqlDbType.NChar;
                    case ConsoleKey.L:
                        return SqlDbType.NText;
                    case ConsoleKey.M:
                        return SqlDbType.NVarChar;
                    case ConsoleKey.N:
                        return SqlDbType.Real;
                    case ConsoleKey.O:
                        return SqlDbType.UniqueIdentifier;
                    case ConsoleKey.P:
                        return SqlDbType.SmallDateTime;
                    case ConsoleKey.Q:
                        return SqlDbType.SmallInt;
                    case ConsoleKey.R:
                        return SqlDbType.SmallMoney;
                    case ConsoleKey.S:
                        return SqlDbType.Text;
                    case ConsoleKey.T:
                        return SqlDbType.Timestamp;
                    case ConsoleKey.U:
                        return SqlDbType.TinyInt;
                    case ConsoleKey.V:
                        return SqlDbType.VarBinary;
                    case ConsoleKey.W:
                        return SqlDbType.VarChar;
                    case ConsoleKey.X:
                        return SqlDbType.Variant;
                    case ConsoleKey.Y:
                        return SqlDbType.Xml;
                    case ConsoleKey.Z:
                        return SqlDbType.Udt;
                    case ConsoleKey.Enter:
                        return SqlDbType.Structured;
                    case ConsoleKey.Delete:
                        return SqlDbType.Date;
                    case ConsoleKey.Backspace:
                        return SqlDbType.Time;
                    case ConsoleKey.Escape:
                        return SqlDbType.DateTime2;
                    case ConsoleKey.Tab:
                        return SqlDbType.DateTimeOffset;
                }
            }
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

        public void WriteParametersResult(SqlParameterCollection sqlParameterCollection)
        {
            foreach (SqlParameter parameter in sqlParameterCollection)
            {
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.ReturnValue)
                {
                    Console.WriteLine($"{parameter.ParameterName} = {parameter.Value}");
                }
            }
        }
        public void SelectContinuation(CancellationTokenSource cancellationTokenSource, AutoResetEvent autoResetCommand)
        {
            Console.WriteLine("If you want to stop the operation, click 'b'");
            if (Console.ReadKey(true).Key == ConsoleKey.B)
            {
                cancellationTokenSource.Cancel(false);
                autoResetCommand.WaitOne();
            }
            else
            {
                autoResetCommand.WaitOne();
            }
        }
    }
}
