using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using static API.Entities.ExcelModels;
using static API.Entities.MediaModels;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl,
                     opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age,
                     opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<RegisterIpDto, AppIp>();
            CreateMap<AppIp, IpDto>().ForMember(dest => dest.Age,
                     opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<RegisterSapDto, AppSap>();
            CreateMap<AppSap, SapDto>().ForMember(dest => dest.Age,
                     opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<AppDocumentation, DocumentationDto>()
           .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.Select(v => new VideoDto
           {
               PublicId = v.PublicId,
               Url = v.Url,

           })))
           .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files.Select(f => new FilesDto
           {
               PublicId = f.PublicId,
               Url = f.Url
           })))
           .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.Pictures.Select(p => new PictureDto
           {
               PublicId = p.PublicId,
               Url = p.Url,

           })));

            CreateMap<DocumentationDto, AppDocumentation>();
            // .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.Select(v => new Video
            // {
            //     Url = v.FileName  // Assuming 'Url' in Video corresponds to the file name
            // })))
            // .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files.Select(f => new Files
            // {
            //     Url = f.FileName  // Assuming 'Url' in Files corresponds to the file name
            // })))
            // .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.Pictures.Select(p => new Picture
            // {
            //     Url = p.FileName  // Assuming 'Url' in Picture corresponds to the file name
            // })));
            CreateMap<AppDocumentation, GetDocsDto>()
     .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.Select(v => new VideoDto
     {
         PublicId = v.PublicId,
         Url = v.Url,
     })))
     .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files.Select(f => new FilesDto
     {
         PublicId = f.PublicId,
         Url = f.Url
     })))
     .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.Pictures.Select(p => new PictureDto
     {
         PublicId = p.PublicId,
         Url = p.Url,
     })));

            CreateMap<ExcelData, ExcelDataDto>();
            CreateMap<ExcelDataDto, ExcelData>();
            CreateMap<ExcelData, GetExcelDto>()
            .ForMember(dest => dest.ExcelFiles, opt => opt.MapFrom(src => src.Files.Select(f => new ExcelFileDto
            {
                Url = f.Url,
                PublicId = f.PublicId
            })));

            CreateMap<ExcelFile, ExcelDataDto>();
            CreateMap<IpUpdateDto, AppIp>();
            CreateMap<AppIp, AppIp>();
            CreateMap<SapUpdateDto, AppSap>();
            CreateMap<AppSap, AppSap>();
            CreateMap<AppCourse, CourseDto>().ReverseMap();
            CreateMap<AppSubject, SubjectDto>().ReverseMap();
            CreateMap<AppCourse, AppCourse>();

        }


    }
}