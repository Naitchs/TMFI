using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static API.Entities.MediaModels;
using System.Security.Claims;
using API.Data;
using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Helpers;
using CloudinaryDotNet;

namespace API.Controllers
{
    [Authorize]
    public class DocumentationController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;

        private readonly Cloudinary _cloudinary;
        private readonly DataContext _context;

        public DocumentationController(IUserRepository userRepository, IMapper mapper,
                          IMediaService mediaService, IOptions<CloudinarySettings> config,
                          DataContext context)
        {
            _context = context;

            var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
            _cloudinary = new Cloudinary(acc);
            _mediaService = mediaService;

            _mapper = mapper;
            _userRepository = userRepository;

        }

        [HttpPost("register-documentation")]
        public async Task<ActionResult> RegisterDocumentation([FromForm] DocumentationDto documentationDto, IFormFile files, IFormFile pictures, IFormFile videos)
        {
            try
            {
                var doc = _mapper.Map<AppDocumentation>(documentationDto);
                if (files != null)
                {
                    var uploadedFile = await _mediaService.AddFileAsync(files);
                    if (uploadedFile.Error != null) return BadRequest(uploadedFile.Error.Message);

                    var fileEntity = new Files
                    {
                        Url = uploadedFile.SecureUrl.AbsoluteUri,
                        PublicId = uploadedFile.PublicId,
                    };
                    doc.Files.Add(fileEntity);
                }

                if (pictures != null)
                {
                    var uploadedPicture = await _mediaService.AddPhotoAsync(pictures);
                    if (uploadedPicture.Error != null) return BadRequest(uploadedPicture.Error.Message);

                    var pictureEntity = new Picture
                    {
                        Url = uploadedPicture.SecureUrl.AbsoluteUri,
                        PublicId = uploadedPicture.PublicId,
                    };
                    doc.Pictures.Add(pictureEntity);
                }

                if (videos != null)
                {
                    var uploadedVideo = await _mediaService.AddVideoAsync(videos);
                    if (uploadedVideo.Error != null) return BadRequest(uploadedVideo.Error.Message);

                    var videoEntity = new MediaModels.Video
                    {
                        Url = uploadedVideo.SecureUrl.AbsoluteUri,
                        PublicId = uploadedVideo.PublicId,
                    };
                    doc.Videos.Add(videoEntity);
                }


                _userRepository.Add(doc);

                await _userRepository.SaveAllAsync();

                return Ok("Files uploaded successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }


        [HttpPost("register-files")]
        public async Task<ActionResult> RegisterFiles([FromForm] List<IFormFile> files, [FromForm] string title,
        [FromForm] string description)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest("No files uploaded");
                }

                var uploadedFiles = new List<RawUploadResult>();

                foreach (var file in files)
                {
                    if (file == null)
                    {
                        continue; // Skip null files
                    }

                    var uploadedFile = await _mediaService.AddFileAsync(file);

                    if (uploadedFile.Error != null)
                    {
                        return BadRequest(uploadedFile.Error.Message);
                    }

                    uploadedFiles.Add(uploadedFile);
                }

                var doc = new AppDocumentation
                {
                    Title = title, // Corrected this line
                    Description = description // Corrected this line
                };


                foreach (var uploadedFile in uploadedFiles)
                {
                    var fileEntity = new Files
                    {
                        Url = uploadedFile.SecureUrl.AbsoluteUri,
                        PublicId = uploadedFile.PublicId,
                    };
                    doc.Files.Add(fileEntity);
                }

                _userRepository.Add(doc);
                await _userRepository.SaveAllAsync();

                return Ok(new { message = "Files uploaded successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("register-pictures")]
        public async Task<ActionResult> RegisterPictures([FromForm] List<IFormFile> pictures, [FromForm] string title,
            [FromForm] string description)
        {
            try
            {
                if (pictures == null || !pictures.Any())
                {
                    return BadRequest("No pictures uploaded");
                }

                var uploadedPictures = new List<ImageUploadResult>();

                foreach (var picture in pictures)
                {
                    if (picture == null)
                    {
                        continue; // Skip null pictures
                    }

                    var uploadedPicture = await _mediaService.AddPhotoAsync(picture);

                    if (uploadedPicture.Error != null)
                    {
                        return BadRequest(uploadedPicture.Error.Message);
                    }

                    uploadedPictures.Add(uploadedPicture);
                }


                var doc = new AppDocumentation
                {
                    Title = title, // Corrected this line
                    Description = description // Corrected this line
                };

                foreach (var uploadedPicture in uploadedPictures)
                {
                    var pictureEntity = new Picture
                    {
                        Url = uploadedPicture.SecureUrl.AbsoluteUri,
                        PublicId = uploadedPicture.PublicId,
                    };
                    doc.Pictures.Add(pictureEntity);
                }

                _userRepository.Add(doc);
                await _userRepository.SaveAllAsync();

                return Ok(new { message = "Pictures uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost("register-videos")]
        public async Task<ActionResult> RegisterVideos([FromForm] List<IFormFile> videos, [FromForm] string title,
          [FromForm] string description)
        {
            try
            {
                if (videos == null || !videos.Any())
                {
                    return BadRequest("No videos uploaded");
                }

                var uploadedVideos = new List<VideoUploadResult>();

                foreach (var video in videos)
                {
                    if (video == null)
                    {
                        continue; // Skip null files
                    }

                    var uploadedVideo = await _mediaService.AddVideoAsync(video);

                    if (uploadedVideo.Error != null)
                    {
                        return BadRequest(uploadedVideo.Error.Message);
                    }

                    uploadedVideos.Add(uploadedVideo);
                }


                var doc = new AppDocumentation
                {
                    Title = title, // Corrected this line
                    Description = description // Corrected this line
                };

                foreach (var uploadedVideo in uploadedVideos)
                {
                    var videoEntity = new MediaModels.Video
                    {
                        Url = uploadedVideo.SecureUrl.AbsoluteUri,
                        PublicId = uploadedVideo.PublicId,
                    };
                    doc.Videos.Add(videoEntity);
                }

                _userRepository.Add(doc);
                await _userRepository.SaveAllAsync();

                return Ok(new { message = "Videos uploaded successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // [HttpPost("register-media")]
        // public async Task<ActionResult> RegisterMedia(
        //     [FromForm] List<IFormFile> files, 
        //     [FromForm] List<IFormFile> pictures,
        //     [FromForm] List<IFormFile> videos,
        //     [FromForm] string title,  
        //     [FromForm] string description) 
        // {
        //     try
        //     {
        //         // Create a single instance of AppDocumentation
        //         var doc = new AppDocumentation
        //         {
        //             Title = title,
        //             Description = description
        //         };

        //         // Process files
        //         foreach (var file in files)
        //         {
        //             if (file == null)
        //             {
        //                 continue; // Skip null files
        //             }

        //             var uploadedFile = await _mediaService.AddFileAsync(file);

        //             if (uploadedFile.Error != null)
        //             {
        //                 return BadRequest(uploadedFile.Error.Message);
        //             }

        //             var fileEntity = new Files
        //             {
        //                 Url = uploadedFile.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedFile.PublicId,
        //             };
        //             doc.Files.Add(fileEntity);
        //         }

        //         // Process pictures
        //         foreach (var picture in pictures)
        //         {
        //             if (picture == null)
        //             {
        //                 continue; // Skip null pictures
        //             }

        //             var uploadedPicture = await _mediaService.AddPhotoAsync(picture);

        //             if (uploadedPicture.Error != null)
        //             {
        //                 return BadRequest(uploadedPicture.Error.Message);
        //             }

        //             var pictureEntity = new Picture
        //             {
        //                 Url = uploadedPicture.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedPicture.PublicId,
        //             };
        //             doc.Pictures.Add(pictureEntity);
        //         }

        //         // Process videos
        //         foreach (var video in videos)
        //         {
        //             if (video == null)
        //             {
        //                 continue; // Skip null videos
        //             }

        //             var uploadedVideo = await _mediaService.AddVideoAsync(video);

        //             if (uploadedVideo.Error != null)
        //             {
        //                 return BadRequest(uploadedVideo.Error.Message);
        //             }

        //             var videoEntity = new MediaModels.Video
        //             {
        //                 Url = uploadedVideo.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedVideo.PublicId,
        //             };
        //             doc.Videos.Add(videoEntity);
        //         }

        //         // Add the single AppDocumentation instance to the repository
        //         _userRepository.Add(doc);
        //         await _userRepository.SaveAllAsync();

        //         return Ok(new { message = "Media uploaded successfully" });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Internal server error: " + ex.Message);
        //     }
        // }


        [HttpGet("get-documentations")]
        public async Task<ActionResult<IEnumerable<GetDocsDto>>> GetDocumentations()
        {
            try
            {
                // Retrieve the list of documentations from your repository
                var documentations = await _userRepository.GetDocsDtoAsync(); // Adjust this based on your repository method

                return Ok(documentations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("get-documentation/{publicId}")]
        public async Task<ActionResult<IEnumerable<GetDocsDto>>> GetDocumentation(string publicId)
        {
            try
            {
                // Retrieve the list of documentations from your repository
                var documentations = await _userRepository.GetDocsDtoByIdAsync(publicId);


                return Ok(documentations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        public int GetNextPublicId()
        {
            int nextId = _context.AppDocumentations.Any() ? _context.AppDocumentations.Max(e => e.Id) + 1 : 001;
            return nextId;
        }

        public string GeneratePublicId()
        {
            int nextId = GetNextPublicId();
            return "TMFIdcmnt" + nextId.ToString("D3");
        }

        [HttpPost("register-media")]
        public async Task<ActionResult> RegisterMedia([FromForm] DocumentationDto dto)
        {
            try
            {
                string publicId = GeneratePublicId();

                var doc = new AppDocumentation
                {
                    PublicId = publicId,
                    Title = dto.Title,
                    Description = dto.Description,
                    AddedDateTime = dto.AddedDateTime
                };

                // Process files
                if (dto.Files != null)
                {
                    foreach (var file in dto.Files)
                    {
                        var uploadedFile = await _mediaService.AddFileAsync(file);

                        if (uploadedFile.Error != null)
                        {
                            return BadRequest(uploadedFile.Error.Message);
                        }

                        var fileEntity = new Files
                        {
                            Url = uploadedFile.SecureUrl.AbsoluteUri,
                            PublicId = uploadedFile.PublicId,
                        };
                        doc.Files.Add(fileEntity);
                    }
                }

                // Process pictures
                if (dto.Pictures != null)
                {
                    foreach (var picture in dto.Pictures)
                    {
                        var uploadedPicture = await _mediaService.AddPhotoAsync(picture);

                        if (uploadedPicture.Error != null)
                        {
                            return BadRequest(uploadedPicture.Error.Message);
                        }

                        var pictureEntity = new Picture
                        {
                            Url = uploadedPicture.SecureUrl.AbsoluteUri,
                            PublicId = uploadedPicture.PublicId,
                        };
                        doc.Pictures.Add(pictureEntity);
                    }
                }

                // Process videos
                if (dto.Videos != null)
                {
                    foreach (var video in dto.Videos)
                    {
                        var uploadedVideo = await _mediaService.AddVideoAsync(video);

                        if (uploadedVideo.Error != null)
                        {
                            return BadRequest(uploadedVideo.Error.Message);
                        }

                        var videoEntity = new MediaModels.Video
                        {
                            Url = uploadedVideo.SecureUrl.AbsoluteUri,
                            PublicId = uploadedVideo.PublicId,
                        };
                        doc.Videos.Add(videoEntity);
                    }
                }

                _userRepository.Add(doc);
                await _userRepository.SaveAllAsync();

                return Ok(new { message = "Media uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




        // [HttpPost("register-medias")]
        // public async Task<ActionResult> RegisterMedia([FromForm] DocumentationDto dto) 
        // {
        //     try
        //     {
        //         var doc = new AppDocumentation
        //         {
        //             Title = dto.Title,
        //             Description = dto.Description
        //         };

        //         // Process files
        //         foreach (var file in dto.Files)
        //         {
        //             var uploadedFile = await _mediaService.AddFileAsync(file);

        //             if (uploadedFile.Error != null)
        //             {
        //                 return BadRequest(uploadedFile.Error.Message);
        //             }

        //             var fileEntity = new Files
        //             {
        //                 Url = uploadedFile.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedFile.PublicId,
        //             };
        //             doc.Files.Add(fileEntity);
        //         }

        //         // Process pictures
        //         foreach (var picture in dto.Pictures)
        //         {
        //             var uploadedPicture = await _mediaService.AddPhotoAsync(picture);

        //             if (uploadedPicture.Error != null)
        //             {
        //                 return BadRequest(uploadedPicture.Error.Message);
        //             }

        //             var pictureEntity = new Picture
        //             {
        //                 Url = uploadedPicture.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedPicture.PublicId,
        //             };
        //             doc.Pictures.Add(pictureEntity);
        //         }

        //         // Process videos
        //         foreach (var video in dto.Videos)
        //         {
        //             var uploadedVideo = await _mediaService.AddVideoAsync(video);

        //             if (uploadedVideo.Error != null)
        //             {
        //                 return BadRequest(uploadedVideo.Error.Message);
        //             }

        //             var videoEntity = new MediaModels.Video
        //             {
        //                 Url = uploadedVideo.SecureUrl.AbsoluteUri,
        //                 PublicId = uploadedVideo.PublicId,
        //             };
        //             doc.Videos.Add(videoEntity);
        //         }

        //         _userRepository.Add(doc);
        //         await _userRepository.SaveAllAsync();

        //         return Ok(new { message = "Media uploaded successfully" });
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Internal server error: " + ex.Message);
        //     }
        // }



    }
}