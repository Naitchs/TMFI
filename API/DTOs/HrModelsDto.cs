namespace API.DTOs
{
    public class HrModelsDto
    {
        public class UploadCertDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string CertType { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadDate { get; set; }
            public List<IFormFile> CertFile { get; set; }
        }

        public class GetCertDto
        {
            public string Title { get; set; }
            public string CertType { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadDate { get; set; }
            public List<HrFileDto> CertFiles { get; set; } = new List<HrFileDto>();
        }

        public class UploadMemoDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string MemoType { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadDate { get; set; }
            public List<IFormFile> MemoFile { get; set; }
        }

        public class HrFileDto
        {

            public string FileType { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadDate { get; set; }
        }


    }
}