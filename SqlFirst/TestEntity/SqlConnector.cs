using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class SqlConnector
    {
        public void Connect(Action<UniversityContext> action)
        {
            using (var universityContext = new UniversityContext())
            {
                action(universityContext);
            } 
        }
    }
}
