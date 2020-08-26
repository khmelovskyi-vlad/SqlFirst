using System;
using System.Collections.Generic;
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
        public (string, DataOutputWays) ReadCommandInformation()
        {
            List<StringBuilder> stringBuilders = new List<StringBuilder>();
            var stringBuilder = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (CreateSql(stringBuilder, stringBuilders), DataOutputWays.executeReader);
                }
                else if ((key.Modifiers & ConsoleModifiers.Shift) != 0 && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (CreateSql(stringBuilder, stringBuilders), DataOutputWays.executeNoQuery);
                }
                else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && (key.Modifiers & ConsoleModifiers.Shift) != 0 && key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return (CreateSql(stringBuilder, stringBuilders), DataOutputWays.executeScalar);
                }
                else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key == ConsoleKey.P)
                {
                    Console.WriteLine();
                    return (CreateSql(stringBuilder, stringBuilders), DataOutputWays.executeProcedure);
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
            WriteColumnsNames(sqlDataReader);
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
        private void WriteColumnsNames(SqlDataReader sqlDataReader)
        {
            var columns = sqlDataReader.GetColumnSchema();
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
        public void WriteCountAffectedRows(int rowsCount)
        {
            Console.WriteLine($"{rowsCount} row(s) affected");
        }

        public void WriteScalar(object value)
        {
            Console.WriteLine(value);
        }
        public void AddParameters(SqlCommand sqlCommand)
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
            sqlCommand.Parameters.AddRange(sqlParameters);
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
                    return Convert.ToInt32(line);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bed input, try again");
                }
            }
        }
    }
}
