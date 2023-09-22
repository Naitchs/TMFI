using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class DocumentationDto
    {

        [Required]public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AddedDateTime { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<IFormFile> Pictures { get; set; } 
        public List<IFormFile> Videos { get; set; }  
    }
}