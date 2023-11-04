using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CourseService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }


        public void AddCourse(AppCourse course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void AddSubject(AppSubject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Where(c => c.Id == id)
                                         .FirstOrDefault();
            course.CourseSubjects = null;
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

        }

        public void DeleteSubject(int id)
        {
            var subject = _context.Subjects.Where(s => s.Id == id)
                                      .FirstOrDefault();
            subject.CourseSubjects = null;
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
            }
        }

        public async Task<AppCourse> GetCourseById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<AppCourse> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<AppCourse> GetCourseId(int id)
        {
              return await _context.Courses
                    .Where(c => c.Id == id)
                    .ProjectTo<AppCourse>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppCourse>> GetCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<AppSubject> GetSubjectById(int id)
        {
            return await _context.Subjects.FindAsync(id);
        }

        public async Task<IEnumerable<AppSubject>> GetSubjectsAsync()
        {
            return await _context.Subjects.ToListAsync();
        }

        public void UpdateCourse(AppCourse course)
        {
            var updateCourse = _context.Courses.Where(c => c.Id == course.Id).FirstOrDefault();

            if (course != null)
            {
                updateCourse.Description = course.Description;
                updateCourse.Title = course.Title;
                updateCourse.Tags = course.Tags;
                updateCourse.DurationInHours = course.DurationInHours;
                _context.SaveChanges();
            }
        }

        public void UpdateSubject(AppSubject subject)
        {
            var updateSubject = _context.Subjects.Where(s => s.Id == subject.Id).FirstOrDefault();

            if (subject != null)
            {
                updateSubject.Description = subject.Description;
                updateSubject.SubjectCode = subject.SubjectCode;
                updateSubject.Name = subject.Name;
                updateSubject.Phase = subject.Phase;
                _context.SaveChanges();
            }
        }
    }
}