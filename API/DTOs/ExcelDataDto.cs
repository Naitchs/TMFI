using static API.Entities.ExcelModels;

namespace API.DTOs
{
    public class ExcelDataDto
    {
    public string PublicId { get; set; }
    public string Title { get; set; }
    public DateTime DateUploaded { get; set; }
    // public List<ExcelDataRow> ExcelDataRowList { get; set; }
    // public Dictionary<string, List<ExcelDataRow>> SheetData { get; set; }
      public List<IFormFile> ExcelFile { get; set; }
    }
}