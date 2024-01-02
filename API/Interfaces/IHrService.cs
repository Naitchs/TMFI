using static API.DTOs.HrModelsDto;
using static API.Entities.HrModels;

namespace API.Interfaces
{
    public interface IHrService
    {
        Task<bool> SaveAllAsync();
        void AddCert(Certificates cert);
        Task<bool> DeleteCertByIdAsync(int id);
        Task<List<HrFiles>> GetHrFilesByCertIdAsync(int certId);
        void DeleteCert(Certificates cert);
        Task<IEnumerable<GetCertDto>> GetAllCertAsync();
        Task<Certificates> GetCertByIdAsync(int id);
        void AddMemo(Memos memo);


    }
}