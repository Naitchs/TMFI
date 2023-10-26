using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
        
    }

        public void Add(AppIp ip)
        {
             _context.Ips.Add(ip);
        }

        public void Add(AppSap sap)
        {
             _context.Saps.Add(sap);
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

        public async Task<GetDocsDto> GetDocsDtoByIdAsync(int id)
        {
            return await _context.AppDocumentations
                    .Where(ip => ip.Id == id)
                    .ProjectTo<GetDocsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
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

       public async Task<IpDto> GetIpsByIdAsync(int id)
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

        public async Task<SapDto> GetSapsByIdAsync(int id)
        {
            return await _context.Saps
                    .Where(ip => ip.Id == id)
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

        public void Update(AppIp ip)
        {
            _context.Entry(ip).State = EntityState.Modified;
        }

        public void Update(AppSap sap)
        {
            throw new NotImplementedException();
        }

        public void update(AppDocumentation doc)
        {
            throw new NotImplementedException();
        }


    }
}