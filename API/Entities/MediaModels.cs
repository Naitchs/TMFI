namespace API.Entities
{
    public class MediaModels
    {
        public class Video
        {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        // Add this property to establish the relationship
        public int AppDocumentationId { get; set; }
        public AppDocumentation AppDocumentation { get; set; }
        }

        public class Files
        {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        // Add this property to establish the relationship
        public int AppDocumentationId { get; set; }
        public AppDocumentation AppDocumentation { get; set; }
        }

        public class Picture
        {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }


        // Add this property to establish the relationship
        public int AppDocumentationId { get; set; }
        public AppDocumentation AppDocumentation { get; set; }
        }


        }
}