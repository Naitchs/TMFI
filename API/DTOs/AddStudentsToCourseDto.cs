namespace API.DTOs
{
    public class AddStudentsToCourseDto
    {
        public int CourseId { get; set; }
        public List<int> StudentIds { get; set; }
    }
}