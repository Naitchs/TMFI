namespace API.DTOs
{
    public class RemoveSubjectsFromCourseDto
    {
        public int CourseId { get; set; }
        public List<int> SubjectIds { get; set; }
    }
}