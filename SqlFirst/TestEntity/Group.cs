using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Group
    {
        [Key]
        public Guid Id { get; set; }
        //[Column(TypeName = "varchar(60)")]
        [MaxLength(60)]
        public string Name { get; set; }
        public int? AverageScore { get; set; }


        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

        public List<Student> Students { get; set; }
    }
}
