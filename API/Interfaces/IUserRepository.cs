using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
  public interface IUserRepository
    {
        void Update(AppUser user);
        void Update(AppIp ip);
        void Add(AppIp ip);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<IpDto>> GetIpsAsync();

        Task<AppUser> GetUsersByIdAsync(int id);
        Task<IpDto> GetIpsByIdAsync(int id);

        Task<AppUser> GetUsersByUsernameAsync(string username);
 
        Task<MemberDto> GetMemberAsync(string username);
    }
}