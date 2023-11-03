namespace API.DTOs
{
    public class GetDocsDto
    {

     
            public string PublicId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime AddedDateTime { get; set; }
            public List<FilesDto> Files { get; set; }
            public List<PictureDto> Pictures { get; set; }
            public List<VideoDto> Videos { get; set; }
    }
}