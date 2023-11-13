using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enums;

namespace API.Entities
{
    public class AppSubject
    {
        public int Id { get; set; }

        public string SubjectCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public PhaseEnum Phase { get; set; }

        public virtual ICollection<CourseSubject> CourseSubjects { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}