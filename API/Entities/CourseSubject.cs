namespace API.Entities
{
    public class CourseSubject
    {

        public int Id { get; set; }
        public int CourseId { get; set; }
        public AppCourse Course { get; set; }

        public int SubjectId { get; set; }
        public AppSubject Subject { get; set; }
    }
}