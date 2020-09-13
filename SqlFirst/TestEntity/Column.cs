using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Column
    {
        public Column(string name, Field field)
        {
            Name = name;
            Field = field;
        }
        public string Name { get; set; }
        public Field Field { get; set; }
    }
}
