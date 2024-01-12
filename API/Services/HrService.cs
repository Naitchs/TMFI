using API.Data;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using static API.DTOs.HrModelsDto;
using static API.Entities.HrModels;

namespace API.Services
{
    public class HrService : IHrService
    {

        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly DataContext _context;
        public HrService(IMapper mapper,
                          IMediaService mediaService,
                          DataContext context)
        {
            _mediaService = mediaService;
            _mapper = mapper;
            _context = context;

        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void AddCert(Certificates cert)
        {
            _context.Certificates.Add(cert);
        }

        public async Task<bool> DeleteCertByIdAsync(int id)
        {
            try
            {
                var hrFile = await _context.HrFiles.FindAsync(id);
                if (hrFile == null)
                {
                    return false;
                }

                _context.HrFiles.Remove(hrFile);
                return await SaveAllAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<HrFiles>> GetHrFilesByCertIdAsync(int certId)
        {
            var hrFiles = await _context.HrFiles
                          .Where(x => x.CertId == certId)
                          .ToListAsync();
            return hrFiles;
        }

        public void DeleteCert(Certificates cert)
        {
            // Check if the certificate entity is not null
            if (cert != null)
            {
                // Remove the certificate entity from the DbSet
                _context.Certificates.Remove(cert);
            }
            else
            {
                throw new ArgumentNullException(nameof(cert), "Certificate entity cannot be null.");
            }
        }

        public void UpdateCert(Certificates certificate)
        {
            _context.Certificates.Update(certificate);
        }

        public async Task<Certificates> FindCertByIdAsync(int certificateId)
        {
            return await _context.Certificates.FindAsync(certificateId);
        }

        public async Task<IEnumerable<GetCertDto>> GetAllCertAsync()
        {
            var data = await _context.Certificates
               .ProjectTo<GetCertDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(dto => dto.UploadDate)
               .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<GetCertDto>> GetAllBoardResolutionCertsAsync()
        {
            var data = await _context.Certificates
               .Where(c => c.CertType == "Certificate of Board Resolution") // Filtering by CertType
               .ProjectTo<GetCertDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(dto => dto.UploadDate)
               .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<GetCertDto>> GetAllRetirementFundCertsAsync()
        {
            var data = await _context.Certificates
               .Where(c => c.CertType == "Certificate of Retirement fund") // Filtering by CertType
               .ProjectTo<GetCertDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(dto => dto.UploadDate)
               .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<GetCertDto>> GetAllEmploymentCertsAsync()
        {
            var data = await _context.Certificates
               .Where(c => c.CertType == "Certificate of Employment") // Filtering by CertType
               .ProjectTo<GetCertDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(dto => dto.UploadDate)
               .ToListAsync();

            return data;
        }




        public async Task<Certificates> GetCertByIdAsync(int id)
        {
            var cert = await _context.Certificates
                .Where(ed => ed.Id == id)
                .ProjectTo<Certificates>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return cert;
        }



        public void AddMemo(Memos memo)
        {
            _context.Memos.Add(memo);
        }


        public async Task<Memos> GetMemoByIdAsync(int id)
        {
            var memo = await _context.Memos
                .Where(ed => ed.Id == id)
                .ProjectTo<Memos>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return memo;
        }

        public async Task<IEnumerable<GetMemoDto>> GetAllMemoAsync()
        {
            var data = await _context.Memos
               .ProjectTo<GetMemoDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(dto => dto.UploadDate)
               .ToListAsync();

            return data;
        }

        public void UpdateMemo(Memos memo)
        {
            _context.Memos.Update(memo);
        }

        public async Task<Memos> FindMemoByIdAsync(int memoId)
        {
            return await _context.Memos.FindAsync(memoId);
        }
    }
}