using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public virtual AppIp Student { get; set; }
        public virtual AppSubject Subject { get; set; }
    }
}