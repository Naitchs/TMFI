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

        public void AddStudentsToCourse(int courseId, List<int> studentIds)
        {
            var course = _context.Courses
                .Include(c => c.CourseStudents)
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            var studentsToAdd = _context.Ips.Where(s => studentIds.Contains(s.Id)).ToList();

            foreach (var student in studentsToAdd)
            {
                course.CourseStudents.Add(new CourseStudent { Course = course, Student = student });
            }

            _context.SaveChanges();
        }

        public void AddStudentToCourse(int courseId, int studentId)
        {
            var course = _context.Courses
                .Include(c => c.CourseStudents)
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            var studentToAdd = _context.Ips.FirstOrDefault(s => s.Id == studentId);

            if (studentToAdd == null)
            {
                throw new InvalidOperationException($"Student with ID {studentId} not found");
            }

            course.CourseStudents.Add(new CourseStudent { Course = course, Student = studentToAdd });

            _context.SaveChanges();
        }

        public List<AppIp> GetStudentsNotInCourse(int courseId)
        {
            return _context.Ips
                .Where(s => !_context.CourseStudents.Any(cs => cs.CourseID == courseId && cs.StudentID == s.Id))
                .ToList();
        }

        public List<AppIp> SearchStudentsNotInCourse(int courseId, string search = null)
        {
            var query = _context.Ips
                .Where(s => !_context.CourseStudents.Any(cs => cs.CourseID == courseId && cs.StudentID == s.Id));

            if (!string.IsNullOrEmpty(search))
            {
                // Add your search conditions based on your model properties
                query = query.Where(s => s.Firstname.Contains(search) || s.Lastname.Contains(search) || s.PublicId.Contains(search));
            }

            return query.ToList();
        }


        public List<AppIp> GetStudentsInCourse(int courseId)
        {
            return _context.Ips
                .Where(s => _context.CourseStudents.Any(cs => cs.CourseID == courseId && cs.StudentID == s.Id))
                .ToList();
        }

        public void RemoveStudentFromCourse(int courseId, int studentId)
        {
            var course = _context.Courses
                .Include(c => c.CourseStudents)
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            var studentToRemove = course.CourseStudents.FirstOrDefault(cs => cs.StudentID == studentId);

            if (studentToRemove != null)
            {
                course.CourseStudents.Remove(studentToRemove);
                _context.SaveChanges();
            }
        }

        public List<AppCourse> GetCoursesOfStudent(int studentId)
        {
            var coursesOfStudent = _context.CourseStudents
                .Include(cs => cs.Course)
                .Where(cs => cs.StudentID == studentId)
                .Select(cs => cs.Course)
                .ToList();

            return coursesOfStudent;
        }

        public void RemoveCourseFromStudent(int studentId, int courseId)
        {
            var enrollment = _context.CourseStudents
                .FirstOrDefault(cs => cs.StudentID == studentId && cs.CourseID == courseId);

            if (enrollment == null)
            {
                throw new InvalidOperationException($"Enrollment not found");
            }

            _context.CourseStudents.Remove(enrollment);
            _context.SaveChanges();
        }

        public void CreateAttendance(CreateAttendanceDto dto)
        {
            var attendance = new Attendance
            {
                StudentID = dto.StudentId,
                SubjectID = dto.SubjectId,
                Date = DateTime.Now,
                Status = dto.Status
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();
        }

        public void DeleteAttendance(int attendanceId)
        {
            var attendance = _context.Attendances.Find(attendanceId);

            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Attendance with ID {attendanceId} not found");
            }
        }



    }
}