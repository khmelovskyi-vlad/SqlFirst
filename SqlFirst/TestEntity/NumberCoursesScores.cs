using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class NumberCourseScores
    {
        public NumberCourseScores(int courseName, int count)
        {
            CourseName = courseName;
            Count = count;
        }
        public int CourseName { get; set; }
        public int Count { get; set; }
    }
}
