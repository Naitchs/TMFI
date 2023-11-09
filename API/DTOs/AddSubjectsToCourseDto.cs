namespace API.DTOs
{
    public class AddSubjectsToCourseDto
    {
        public int CourseId { get; set; }
         public List<int> SubjectIds {get; set;}
    }
}