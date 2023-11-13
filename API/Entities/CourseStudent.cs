namespace API.Entities
{
    public class CourseStudent
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public virtual AppCourse Course { get; set; }
        public virtual AppIp Student { get; set; }
    }
}