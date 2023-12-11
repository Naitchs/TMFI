using API.DTOs;
using static API.Entities.ExcelModels;

namespace API.Interfaces
{
    public interface IExcelService
    {

        
        //  void ProcessUploadedFile(string filePath);
        // List<ExcelDataRow> ProcessUploadedFile(string filePath);

        Dictionary<string, List<ExcelDataRow>> ProcessUploadedFile(string filePath);
        // void SaveExcelDataToDatabase(ExcelDataDto excelDataDto);
        // void SaveExcelDataToDatabase(ExcelDataDto excelDataDto, Dictionary<string, List<ExcelDataRow>> sheetData);
        Task<IEnumerable<GetExcelDto>> GetExcelDataAsync();

        Task<GetExcelDto> GetExcelDataByPublicIdAsync(string publicId);

        Task<bool> SaveAllAsync();
        void Add(ExcelData excelData);
        void DeleteData(int id);
    }
}