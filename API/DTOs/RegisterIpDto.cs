using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterIpDto
    {
        // public int Id { get; set; }

        public string PublicId { get; set; }

        [Required] public string Firstname { get; set; }
        
        public string Middlename { get; set; }

        [Required] public string Lastname { get; set; }

        public string Suffix { get; set; }

        [Required] public DateOnly? DateOfBirth { get; set; }

        [Required] public string Gender { get; set; }

        [Required] public string Status { get; set; }

        [Required] public string Tribe { get; set; }

        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public string Facebook { get; set; }

        public string Street { get; set; }

        [Required] public string Barangay { get; set; }

        [Required] public string City { get; set; }

        [Required] public string ChurchName { get; set; }

        public string Ministry { get; set; }

        public string Reason { get; set; }
    }
}