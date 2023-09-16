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

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<IpDto>> GetIpsAsync();
        Task<IEnumerable<SapDto>> GetSapsAsync();

        Task<AppUser> GetUsersByIdAsync(int id);
        Task<IpDto> GetIpsByIdAsync(int id);
        Task<SapDto> GetSapsByIdAsync(int id);

        Task<AppUser> GetUsersByUsernameAsync(string username);
 
        Task<MemberDto> GetMemberAsync(string username);
        
    }
}