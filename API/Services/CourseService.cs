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


        public AppCourse AddCourse(AppCourse course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();

            return course;
        }

        public AppSubject AddSubject(AppSubject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            return subject;

        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Where(c => c.Id == id)
                                         .FirstOrDefault();
            // var course = _context.Courses.SingleOrDefault(c => c.Id == id);
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


        public void AddSubjectsToCourse(int courseId, List<int> subjectIds)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            var subjectsToAdd = _context.Subjects.Where(s => subjectIds.Contains(s.Id)).ToList();

            foreach (var subject in subjectsToAdd)
            {
                course.CourseSubjects.Add(new CourseSubject { Course = course, Subject = subject });
            }

            _context.SaveChanges();

        }


        // public List<AppSubject> GetSubjectsInCourse(int courseId)
        // {
        //     var course = _context.Courses
        //         .Include(c => c.CourseSubjects)
        //         .ThenInclude(cs => cs.Subject)
        //         .FirstOrDefault(c => c.Id == courseId);

        //     if (course == null)
        //     {
        //         throw new InvalidOperationException($"Course with ID {courseId} not found");
        //     }

        //     return course.CourseSubjects.Select(cs => cs.Subject).ToList();
        // }


        private List<AppSubject> GetSubjectsNotInCourse(int courseId, List<int> subjectIds)
        {
            return _context.Subjects
                .Where(s => subjectIds.Contains(s.Id) && !_context.CourseSubjects.Any(cs => cs.CourseID == courseId && cs.SubjectID == s.Id))
                .ToList();
        }

        public List<AppSubject> GetSubjectsNotInCourse(int courseId)
        {
            return _context.Subjects
                .Where(s => !_context.CourseSubjects.Any(cs => cs.CourseID == courseId && cs.SubjectID == s.Id))
                .ToList();
        }

        public List<AppSubject> GetSubjectsInCourse(int courseId)
        {
            var subjectsInCourse = _context.CourseSubjects
                .Where(cs => cs.CourseID == courseId)
                .Select(cs => cs.Subject)
                .ToList();

            return subjectsInCourse;
        }


        public void UpdateAddSubjectsToCourse(int courseId, List<int> subjectIds)
        {
            var course = _context.Courses
           .Include(c => c.CourseSubjects)
           .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            // Add new subjects to the course
            var subjectsToAdd = GetSubjectsNotInCourse(courseId, subjectIds);

            foreach (var subjectToAdd in subjectsToAdd)
            {
                course.CourseSubjects.Add(new CourseSubject { Course = course, Subject = subjectToAdd });
            }

            _context.SaveChanges();
        }


        public void RemoveSubjectFromCourse(int courseId, int subjectId)
        {
            var course = _context.Courses
                .Include(c => c.CourseSubjects)
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            // Remove the specific subject
            var subjectToRemove = course.CourseSubjects.FirstOrDefault(cs => cs.SubjectID == subjectId);

            if (subjectToRemove != null)
            {
                course.CourseSubjects.Remove(subjectToRemove);
                _context.SaveChanges();
            }
        }









        // public void AddSubjectsToCourse(int courseId, List<int> subjectIds)
        // {
        //     var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);

        //     if (course == null)
        //     {
        //         throw new InvalidOperationException($"Course with ID {courseId} not found");
        //     }

        //     foreach (var subjectId in subjectIds)
        //     {
        //         var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);

        //         if (subject == null)
        //         {
        //             throw new InvalidOperationException($"Subject with ID {subjectId} not found");
        //         }

        //         if (subject.Courses == null)
        //         {
        //             subject.Courses = new List<AppCourse>();
        //         }

        //         subject.Courses.Add(course);
        //     }

        //     _context.SaveChanges();
        // }

        // public void AddSubjectsToCourse(int courseId, List<int> subjectIds)
        // {
        //     var course = _context.Courses
        //         .Include(c => c.Subjects)
        //         .FirstOrDefault(c => c.Id == courseId);

        //     if (course == null)
        //     {
        //         throw new Exception("Course not found");
        //     }

        //     foreach (var subjectId in subjectIds)
        //     {
        //         var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);

        //         if (subject != null)
        //         {
        //             course.Subjects.Add(subject);
        //         }
        //     }

        //     _context.SaveChanges();
        // }


    }
}