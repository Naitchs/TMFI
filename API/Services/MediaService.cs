using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class MediaService : IMediaService
    {

        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MediaService(IOptions<CloudinarySettings> config, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        // public async Task<RawUploadResult> AddExcelFileAsync(IFormFile file)
        // {
        //       if (file == null || file.Length == 0)
        //         {
        //             throw new Exception("Invalid file");
        //         }

        //         using (var stream = file.OpenReadStream())
        //         {
        //             var uploadParams = new RawUploadParams
        //             {
        //                 File = new FileDescription(file.FileName, stream),
        //                 Folder = "readExcel",
        //                 UseFilename = true,
        //                 AccessMode = "public"
        //             };

        //             return await _cloudinary.UploadLargeAsync(uploadParams);
        //         }
        // }
        public string GetFilePath(string fileName)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
        }


        public async Task<bool> AddExcelFileAsync(IFormFile file, string filename, string fileExtension)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("Invalid file");
                }

                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, filename + fileExtension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        //         public async Task<string> AddExcelFileAsync(IFormFile file)
        // {
        //     if (file == null || file.Length == 0)
        //     {
        //         throw new Exception("Invalid file");
        //     }

        //     var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        //     if (!Directory.Exists(uploadsFolder))
        //     {
        //         Directory.CreateDirectory(uploadsFolder);
        //     }

        //     var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        //     var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //     using (var stream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await file.CopyToAsync(stream);
        //     }

        //     return Path.Combine("uploads", uniqueFileName); // Return relative path to the saved file
        // }




        public async Task<RawUploadResult> AddFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Invalid file");
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "documentation",
                    UseFilename = true,
                    AccessMode = "public"
                };

                return await _cloudinary.UploadLargeAsync(uploadParams);
            }
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Invalid file");
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "documentation",
                    UseFilename = true,
                };

                return await _cloudinary.UploadAsync(uploadParams);
            }
        }

        public async Task<VideoUploadResult> AddVideoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Invalid file");
            }
            string fileExtension = Path.GetExtension(file.FileName);
            string fileType = fileExtension.ToLower() switch
            {
                ".mp4" => "video",
                ".webm" => "video",
                ".mp3" => "audio",
                ".wav" => "audio",
                // Add more file extensions and types as needed
                _ => throw new Exception("Unsupported file type"),
            };

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "documentation",
                    UseFilename = true,
                };

                return await _cloudinary.UploadAsync(uploadParams);
            }
        }

        public Task<DeletionResult> DeleteExcelFileAsync(string publicId)
        {
            throw new NotImplementedException();
        }

        public async Task<DeletionResult> DeleteFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<DeletionResult> DeleteVideoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}