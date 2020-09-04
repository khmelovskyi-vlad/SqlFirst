using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Score
    {
        [Key]
        public Guid Id { get; set; }
        public int Value { get; set; }


        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
