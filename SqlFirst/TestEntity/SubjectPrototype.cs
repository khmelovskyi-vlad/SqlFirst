using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    public class SubjectPrototype
    {
        public SubjectPrototype(string subjectName, int courseName, string specialtyName)
        {
            SubjectName = subjectName;
            CourseName = courseName;
            SpecialtyName = specialtyName;
        }
        public string SubjectName { get; set; }
        public int CourseName { get; set; }
        public string SpecialtyName { get; set; }
    }
}
