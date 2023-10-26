namespace API.DTOs
{
    public class GetExcelDto
    {
           public string PublicId { get; set; }
    public string Title { get; set; }
    public DateTime DateUploaded { get; set; }
     
   public List<ExcelFileDto> ExcelFiles { get; set; } = new List<ExcelFileDto>();
    }
}