using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public int Name { get; set; }


        public List<Score> Scores { get; set; }
        public List<Group> Groups { get; set; }
        public List<SubjectCourse> SubjectCourses { get; set; }
    }
}
