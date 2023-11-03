using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }

        [Required] public string KnownAs { get; set; }

        [Required] public string ActiveStatus { get; set; }
        
        // [Required] public DateOnly? DateOfBirth { get; set; }

        // [Required] public string City { get; set; }

        // [Required] public string Country { get; set; }
        
        [Required]
        [StringLength(15, MinimumLength = 8 )]
        public string Password { get; set; }
    }
}