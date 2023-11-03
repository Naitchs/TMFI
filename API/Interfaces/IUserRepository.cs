using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
  public interface IUserRepository
    {
      
        void Update(AppUser user);

        void UpdateIp(AppIp ip);
        void Add(AppIp ip);

        void UpdateSap(AppSap sap);
        void Add(AppSap sap);

        void Add(AppDocumentation doc);
        void updateDoc(AppDocumentation doc);


        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<IpDto>> GetIpsAsync();
        Task<IEnumerable<SapDto>> GetSapsAsync();
        // Task<IEnumerable<DocumentationDto>> GetDocsAsync();
        Task<IEnumerable<GetDocsDto>> GetDocsDtoAsync();
    

        Task<AppUser> GetUsersByIdAsync(int id);
        Task<IpDto> GetIpsByIdAsync(string publicId);
        Task<AppIp> GetIpsByPublicIdAsync(string publicId);
        Task<AppSap> GetSapByPublicIdAsync(string publicId);
        Task<SapDto> GetSapsByIdAsync(string publicId);
        Task<GetDocsDto> GetDocsDtoByIdAsync(string publicId);

        Task<AppUser> GetUsersByUsernameAsync(string username);
 
 
        Task<MemberDto> GetMemberAsync(string username);
        void Add(MediaModels.Files fileEntity);
        void Add(MediaModels.Picture pictureEntity);
        void Add(MediaModels.Video videoEntity);

        Task<bool> VerifyPasswordAsync(string username, string password);
        Task ChangePasswordAsync(AppUser user, string newPassword);
        // Task<AppIp> GetAppIpByDetails(RegisterIpDto registerIpDto);
         Task<bool> IsDuplicateIpDetailsAsync(RegisterIpDto registerIpDto);
         Task<bool> IsDuplicateSapDetailsAsync(RegisterSapDto registerSapDto);

        int IpGetNextPublicId();
        string IpGeneratePublicId();

        int SapGetNextPublicId();
        string SapGeneratePublicId();
        

        //   Task<AppCourse> GetCourseByIdAsync(int id);
        //   Task<IEnumerable<CourseDto>> GetCoursesAsync();
        //   Task<AppCourse> GetCourseInfoAsync(int id);

        //   Task<AppIp> GetStudentInfoAsync(int id);


        //  Task<AppUser> GetFacilitatorInfoAsync (string username);

        //   Task<IEnumerable<FacilitatorDto>> GetFacilitatorsAsync();
    }
}