namespace API.DTOs
{
    public class DeleteAttendanceRequest
    {
        public int SubjectId { get; set; }
        public List<int> AttendanceIds { get; set; }
    }

}