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

        public void AddCert(Certificates cert)
        {
            _context.Certificates.Add(cert);
        }

        public void AddMemo(Memos memo)
        {
            _context.Memos.Add(memo);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
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



        public async Task<IEnumerable<GetCertDto>> GetAllCertAsync()
        {
            var data = await _context.Certificates
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
    }
}