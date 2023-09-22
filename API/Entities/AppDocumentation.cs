
using static API.Entities.MediaModels;

namespace API.Entities
{
    public class AppDocumentation
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description {get; set;}
            public DateTime AddedDateTime { get; set; } = DateTime.UtcNow; // Initialize with current UTC time

    // Navigation properties
           public ICollection<Video> Videos { get; set; } = new List<Video>();
           public ICollection<Files> Files { get; set; } = new List<Files>();
           public ICollection<Picture> Pictures { get; set; } = new List<Picture>();
    }
}