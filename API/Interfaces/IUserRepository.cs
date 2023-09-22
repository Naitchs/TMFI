using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
  public interface IUserRepository
    {
        void Update(AppUser user);

        void Update(AppIp ip);
        void Add(AppIp ip);

        void Update(AppSap sap);
        void Add(AppSap sap);

        void Add(AppDocumentation doc);
        void update(AppDocumentation doc);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<IpDto>> GetIpsAsync();
        Task<IEnumerable<SapDto>> GetSapsAsync();
        // Task<IEnumerable<DocumentationDto>> GetDocsAsync();
        Task<IEnumerable<GetDocsDto>> GetDocsDtoAsync();
    

        Task<AppUser> GetUsersByIdAsync(int id);
        Task<IpDto> GetIpsByIdAsync(int id);
        Task<SapDto> GetSapsByIdAsync(int id);
        Task<GetDocsDto> GetDocsDtoByIdAsync(int id);

        Task<AppUser> GetUsersByUsernameAsync(string username);
 
        Task<MemberDto> GetMemberAsync(string username);
        void Add(MediaModels.Files fileEntity);
        void Add(MediaModels.Picture pictureEntity);
        void Add(MediaModels.Video videoEntity);
    }
}