namespace API.Entities
{
    public class AppCourse
    {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int DurationInHours { get; set; }

    public string Tags { get; set; }

    public ICollection<CourseSubject> CourseSubjects { get; set; }

    }
}