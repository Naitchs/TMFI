namespace API.Entities
{
    public class AppSap
    {
        public int Id { get; set; }
        
        public string PublicId { get; set; } 

        public string Firstname { get; set; }
        
        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string Suffix { get; set; }

        public string Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string BirthPlaceBrgy { get; set; }

        public string BirthPlaceCity { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Tribe { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public string ParentOccupation { get; set; }

        public int NumberOfSibling { get; set; }

        public string YearSapStarted { get; set; }

        public string GradeLevel { get; set; }

        public string SchoolName { get; set; }

        public string ChurchName { get; set; }

        public string CommunityInvolvement { get; set; }

        public string Talent { get; set; }

        public string Sport { get; set; }

        public string Ambition { get; set; }

        public string Motto { get; set; }

    }
}