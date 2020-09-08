using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class SortInformation
    {
        public List<string> SortColumns = new List<string>();
        public SortType sortType = SortType.InOriginalOrder;
    }
}
