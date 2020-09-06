using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class CommandPrototype
    {
        public CommandPrototype()
        {

        }
        public string Sql { get; set; }

        public SqlParameter[] sqlParameters { get; set; }
    }
}
