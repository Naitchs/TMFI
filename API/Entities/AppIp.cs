namespace API.Entities
{
    public class AppIp
    {
        public int Id { get; set; }

        public string PublicId { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string Suffix { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Status { get; set; }

        public string Tribe { get; set; }

        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public string Facebook { get; set; }

        public string Street { get; set; }

        public string Barangay { get; set; }

        public string City { get; set; }

        public string ChurchName { get; set; }

        public string Ministry { get; set; }

        public string Reason { get; set; }

        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }


    }
}