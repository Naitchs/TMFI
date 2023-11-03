using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterSapDto
    {

        public string PublicId { get; set; }
        
        [Required] public string Firstname { get; set; }
        
        public string Middlename { get; set; }

        [Required] public string Lastname { get; set; }

        public string Suffix { get; set; }

        [Required] public string Gender { get; set; }

        [Required] public DateOnly DateOfBirth { get; set; }

        public string BirthPlaceBrgy { get; set; }

        [Required] public string BirthPlaceCity { get; set; }

        [Required] public string Street { get; set; }

        [Required] public string City { get; set; }

        [Required] public string Tribe { get; set; }

        [Required] public string FatherName { get; set; }

        [Required] public string MotherName { get; set; }

        [Required] public string ParentOccupation { get; set; }

        public int NumberOfSibling { get; set; }

        [Required] public string YearSapStarted { get; set; }

        [Required] public string GradeLevel { get; set; }

        [Required] public string SchoolName { get; set; }

        [Required] public string ChurchName { get; set; }

        public string CommunityInvolvement { get; set; }

        public string Talent { get; set; }

        public string Sport { get; set; }

        public string Ambition { get; set; }

        public string Motto { get; set; }

    }
}