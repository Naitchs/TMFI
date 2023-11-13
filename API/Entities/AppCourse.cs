namespace API.Entities
{
    public class AppCourse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DurationInHours { get; set; }

        public string Tags { get; set; }
        public virtual ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }

    }
}