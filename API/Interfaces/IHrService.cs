using static API.DTOs.HrModelsDto;
using static API.Entities.HrModels;

namespace API.Interfaces
{
    public interface IHrService
    {
        Task<bool> SaveAllAsync();

        //cert
        void AddCert(Certificates cert);
        Task<bool> DeleteCertByIdAsync(int id);
        Task<List<HrFiles>> GetHrFilesByCertIdAsync(int certId);
        void DeleteCert(Certificates cert);
        void UpdateCert(Certificates certificate);
        Task<Certificates> FindCertByIdAsync(int certificateId);
        Task<IEnumerable<GetCertDto>> GetAllCertAsync();
        Task<IEnumerable<GetCertDto>> GetAllBoardResolutionCertsAsync();
        Task<IEnumerable<GetCertDto>> GetAllRetirementFundCertsAsync();
        Task<IEnumerable<GetCertDto>> GetAllEmploymentCertsAsync();
        Task<Certificates> GetCertByIdAsync(int id);

        //memo
        void AddMemo(Memos memo);
        Task<Memos> GetMemoByIdAsync(int id);
        Task<IEnumerable<GetMemoDto>> GetAllMemoAsync();
        void UpdateMemo(Memos memo);
        Task<Memos> FindMemoByIdAsync(int memoId);


    }
}