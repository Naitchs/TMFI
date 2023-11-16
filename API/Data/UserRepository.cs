using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    public UserRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
    {
      _userManager = userManager;
      _mapper = mapper;
      _context = context;

    }

    public AppIp Add(AppIp ip)
    {
      _context.Ips.Add(ip);

      return ip;
    }

    public AppSap Add(AppSap sap)
    {
      _context.Saps.Add(sap);

      return sap;
    }


    public void Add(AppDocumentation doc)
    {
      _context.AppDocumentations.Add(doc);
    }

    public void Add(MediaModels.Files fileEntity)
    {
      _context.Files.Add(fileEntity);
    }

    public void Add(MediaModels.Picture pictureEntity)
    {
      _context.Pictures.Add(pictureEntity);
    }

    public void Add(MediaModels.Video videoEntity)
    {
      _context.Videos.Add(videoEntity);
    }

    public async Task ChangePasswordAsync(AppUser user, string newPassword)
    {
      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
      await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    //      public async Task<AppIp> GetAppIpByDetails(RegisterIpDto registerIpDto)
    // {
    //     return await _context.Ips.FirstOrDefaultAsync(ip =>
    //         ip.Firstname == registerIpDto.Firstname &&
    //         ip.Lastname == registerIpDto.Lastname &&
    //         ip.DateOfBirth == registerIpDto.DateOfBirth &&
    //         ip.City == registerIpDto.City
    //     );
    // }
    public async Task<bool> IsDuplicateIpDetailsAsync(RegisterIpDto registerIpDto)
    {
      return await _context.Ips.AnyAsync(ip =>
          ip.Firstname == registerIpDto.Firstname &&
          ip.Middlename == registerIpDto.Middlename &&
          ip.Lastname == registerIpDto.Lastname &&
          ip.Gender == registerIpDto.Gender &&
          ip.Tribe == registerIpDto.Tribe &&
          ip.DateOfBirth == registerIpDto.DateOfBirth

      );
    }

    public async Task<bool> IsDuplicateSapDetailsAsync(RegisterSapDto registerSapDto)
    {
      return await _context.Saps.AnyAsync(sap =>
          sap.Firstname == registerSapDto.Firstname &&
          sap.Middlename == registerSapDto.Middlename &&
          sap.Lastname == registerSapDto.Lastname &&
          sap.Gender == registerSapDto.Gender &&
          sap.DateOfBirth == registerSapDto.DateOfBirth &&
          sap.Tribe == registerSapDto.Tribe

      );
    }




    // public void Add(AppCourse course)
    // {
    //     _context.Courses.Add(course);
    // }

    // public async Task<AttendanceDto> GetAttendanceByIdAsync(int attendanceId)
    // {
    //      return await _context.Attendances
    // .Include(a => a.Course)
    // .Include(a => a.Student)
    // .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
    // .FirstOrDefaultAsync(a => a.Id == attendanceId);
    // }

    // public async Task<IEnumerable<AttendanceDto>> GetAttendancesAsync()
    // {
    //         return await _context.Attendances
    // .Include(a => a.Course)
    // .Include(a => a.Student)
    // .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
    // .ToListAsync();
    // }

    // public async Task<AppCourse> GetCourseByIdAsync(int id)
    // {
    //                 return await _context.Courses
    //             .Where(course => course.Id == id)
    //             .ProjectTo<AppCourse>(_mapper.ConfigurationProvider)
    //             .FirstOrDefaultAsync();
    // }

    // public async Task<AppCourse> GetCourseInfoAsync(int id)
    // {
    //      return await _context.Courses
    //      .Where(c => c.Id == id)
    //      .ProjectTo<AppCourse>(_mapper.ConfigurationProvider)
    //      .FirstOrDefaultAsync();


    // }

    // public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
    // {
    //                 return await _context.Courses
    //                 .Include(d => d.Enrollments)
    //       .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
    //       .ToListAsync();
    // }

    public async Task<IEnumerable<GetDocsDto>> GetDocsDtoAsync()
    {
      return await _context.AppDocumentations
       .Include(d => d.Files)
       .Include(d => d.Pictures)
       .Include(d => d.Videos)
     .ProjectTo<GetDocsDto>(_mapper.ConfigurationProvider)
     .ToListAsync();
    }

    public async Task<GetDocsDto> GetDocsDtoByIdAsync(string publicId)
    {
      var getDocsDto = await _context.AppDocumentations
   .Where(ed => ed.PublicId == publicId)
   .ProjectTo<GetDocsDto>(_mapper.ConfigurationProvider)
   .FirstOrDefaultAsync();

      return getDocsDto;
    }



    // public Task<EnrollmentDto> GetEnrollmentByIdAsync(int enrollmentId)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync()
    // {
    //     throw new NotImplementedException();
    // }


    // public async Task<AppUser> GetFacilitatorInfoAsync(string username)
    // {
    //     return await _context.Users
    //         .Include(p => p.Photos)
    //         .SingleOrDefaultAsync(x => x.UserName == username);
    // }

    // public async Task<IEnumerable<FacilitatorDto>> GetFacilitatorsAsync()
    // {
    //        return await _context.Users
    //       .ProjectTo<FacilitatorDto>(_mapper.ConfigurationProvider)
    //       .ToListAsync();
    // }

    // public Task<GradeDto> GetGradeByIdAsync(int gradeId)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task<IEnumerable<GradeDto>> GetGradesAsync()
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<IEnumerable<IpDto>> GetIpsAsync()
    {
      return await _context.Ips
       .ProjectTo<IpDto>(_mapper.ConfigurationProvider)
       .ToListAsync();
    }

    public async Task<IpDto> GetIpsByIdAsync(string publicId)
    {

      return await _context.Ips
           .Where(ip => ip.PublicId == publicId)
           .ProjectTo<IpDto>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync();
    }

    public async Task<IpDto> GetStudentByIdAsync(int id)
    {

      return await _context.Ips
           .Where(ip => ip.Id == id)
           .ProjectTo<IpDto>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync();
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
      return await _context.Users
              .Where(x => x.UserName == username)
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
      return await _context.Users
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
              .ToListAsync();
    }

    public async Task<IEnumerable<SapDto>> GetSapsAsync()
    {
      return await _context.Saps
             .ProjectTo<SapDto>(_mapper.ConfigurationProvider)
             .ToListAsync();
    }

    public async Task<SapDto> GetSapsByIdAsync(string publicId)
    {
      return await _context.Saps
              .Where(ip => ip.PublicId == publicId)
              .ProjectTo<SapDto>(_mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
    }


    public async Task<AppIp> GetStudentInfoAsync(int id)
    {
      return await _context.Ips
.Where(s => s.Id == id)
.ProjectTo<AppIp>(_mapper.ConfigurationProvider)
.FirstOrDefaultAsync();
    }

    // public Task<IEnumerable<StudentDto>> GetStudentsAsync()
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
      return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
    }

    public async Task<AppUser> GetUsersByIdAsync(int id)
    {
      return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUsersByUsernameAsync(string username)
    {
      return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);

    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
      _context.Entry(user).State = EntityState.Modified;
    }

    public void UpdateIp(AppIp ip)
    {
      _context.Entry(ip).State = EntityState.Modified;
    }

    public void UpdateSap(AppSap sap)
    {
      _context.Entry(sap).State = EntityState.Modified;
    }

    public void updateDoc(AppDocumentation doc)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> VerifyPasswordAsync(string username, string password)
    {
      var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

      if (user == null) return false;

      return await _userManager.CheckPasswordAsync(user, password);
    }

    public int IpGetNextPublicId()
    {
      int nextId = _context.Ips.Any() ? _context.Ips.Max(e => e.Id) + 1 : 0001;
      return nextId;
    }

    public string IpGeneratePublicId()
    {
      int nextId = IpGetNextPublicId();
      return "ip" + nextId.ToString("D4");
    }

    public int SapGetNextPublicId()
    {
      int nextId = _context.Saps.Any() ? _context.Saps.Max(e => e.Id) + 1 : 0001;
      return nextId;
    }

    public string SapGeneratePublicId()
    {
      int nextId = SapGetNextPublicId();
      return "sap" + nextId.ToString("D4");
    }

    public async Task<AppIp> GetIpsByPublicIdAsync(string publicId)
    {
      return await _context.Ips
         .Where(ip => ip.PublicId == publicId)
         .ProjectTo<AppIp>(_mapper.ConfigurationProvider)
         .FirstOrDefaultAsync();
    }

    public async Task<AppSap> GetSapByPublicIdAsync(string publicId)
    {
      return await _context.Saps
         .Where(ip => ip.PublicId == publicId)
         .ProjectTo<AppSap>(_mapper.ConfigurationProvider)
         .FirstOrDefaultAsync();
    }

    public void DeleteIp(int id)
    {
      var ip = _context.Ips.Where(c => c.Id == id)
                           .FirstOrDefault();
      // var course = _context.Courses.SingleOrDefault(c => c.Id == id);
      if (ip != null)
      {
        _context.Ips.Remove(ip);
        _context.SaveChanges();
      }
    }


    public void DeleteSap(int id)
    {
      var sap = _context.Saps.Where(c => c.Id == id)
                           .FirstOrDefault();
      // var course = _context.Courses.SingleOrDefault(c => c.Id == id);
      if (sap != null)
      {
        _context.Saps.Remove(sap);
        _context.SaveChanges();
      }
    }


  }
}