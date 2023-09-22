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
    }
}