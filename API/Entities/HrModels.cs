namespace API.Entities
{
    public class HrModels
    {
        public class Certificates
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string CertType { get; set; }
            public DateTime UploadDate { get; set; }
            public ICollection<HrFiles> CertFiles { get; set; } = new List<HrFiles>();
        }

        public class Memos
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string MemoType { get; set; }
            public DateTime UploadDate { get; set; }
            public ICollection<HrFiles> MemoFiles { get; set; } = new List<HrFiles>();
        }

        public class HrFiles
        {
            public int Id { get; set; }
            public string FileType { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public DateTime UploadDate { get; set; }
            public int? CertId { get; set; }
            public Certificates Certs { get; set; }

            public int? MemoId { get; set; }
            public Memos Memos { get; set; }


        }
    }
}