using API.Enums;

namespace API.DTOs
{
    public class SubjectDto
    {
    public string SubjectCode {get; set;}

    public string Name { get; set; }

    public string Description { get; set; }
    public PhaseEnum Phase {get; set;}
    }
}