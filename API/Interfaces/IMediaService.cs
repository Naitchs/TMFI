using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IMediaService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);

        Task<RawUploadResult> AddFileAsync(IFormFile file);
        Task<DeletionResult> DeleteFileAsync(string publicId);

        Task<VideoUploadResult> AddVideoAsync(IFormFile file);
        Task<DeletionResult> DeleteVideoAsync(string publicId);

        // Task<RawUploadResult> AddExcelFileAsync(IFormFile file);
        Task<bool> AddExcelFileAsync(IFormFile file, string filename, string fileExtension);

        // FileStream GetFileStream(string fileName);

        string GetFilePath(string fileName);

        //  Task<string> AddExcelFileAsync(IFormFile file);
        Task<DeletionResult> DeleteExcelFileAsync(string publicId);

        Task<bool> AddCertFileAsync(IFormFile file, string filename, string fileExtension);
        void DeleteCertFile(string filename);
        void DeleteCertificate(string fileName);
        Task<bool> AddMemoFileAsync(IFormFile file, string filename, string fileExtension);
    }
}