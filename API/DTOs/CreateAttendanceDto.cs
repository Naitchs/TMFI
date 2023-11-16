namespace API.DTOs
{
    public class CreateAttendanceDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}