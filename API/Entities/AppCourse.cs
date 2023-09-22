namespace API.Entities
{
    public class AppCourse
    {
         public int Id { get; set; }
         public string CourseName { get; set; }
         public string Description { get; set; }
         public DateTime StartDate { get; set; } = DateTime.UtcNow;
         public DateTime EndDate { get; set; }

    }

}