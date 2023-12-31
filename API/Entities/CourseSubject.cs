namespace API.Entities
{
    public class CourseSubject
    {

        public int CourseID { get; set; }
        public int SubjectID { get; set; }
        public virtual AppCourse Course { get; set; }
        public virtual AppSubject Subject { get; set; }
    }
}