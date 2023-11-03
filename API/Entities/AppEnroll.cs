namespace API.Entities
{
    public class AppEnroll
    {
           
    public int Id { get; set; }

  
    public int StudentId { get; set; }

   
    public int CourseId { get; set; }


    public DateTime EnrollmentDate { get; set; }

    // Navigation properties
    public List<AppIp> Student { get; set; }

    public virtual AppCourse Course { get; set; }
    }
}