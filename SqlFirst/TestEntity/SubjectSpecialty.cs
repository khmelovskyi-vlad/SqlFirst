using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    public class SubjectSpecialty
    {
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
        
        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}
