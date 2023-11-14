using API.Data;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICourseService
    {

        AppCourse AddCourse(AppCourse course);

        void UpdateCourse(AppCourse course);

        void DeleteCourse(int id);

        Task<IEnumerable<AppCourse>> GetCoursesAsync();
        Task<IEnumerable<AppSubject>> GetSubjectsAsync();
        Task<AppCourse> GetCourseByIdAsync(int id);

        Task<AppCourse> GetCourseId (int id);


        AppSubject AddSubject(AppSubject subject);
        void UpdateSubject(AppSubject subject);
        void DeleteSubject(int id);
        Task<AppSubject> GetSubjectById(int id);

        // void AddSubjectsToCourse(int subjectId, List<int> courseIds);
        
         void AddSubjectsToCourse(int courseId, List<int> subjectIds);
         List<AppSubject> GetSubjectsInCourse(int courseId);
         List<AppSubject> GetSubjectsNotInCourse(int courseId);
         void UpdateAddSubjectsToCourse(int courseId, List<int> subjectIds);
         void RemoveSubjectFromCourse(int courseId, int subjectId);



         void AddStudentsToCourse(int courseId, List<int> studentIds);
         List<AppIp> GetStudentsNotInCourse(int courseId);
         List<AppIp> GetStudentsInCourse(int courseId);
         void RemoveStudentFromCourse(int courseId, int studentId);
         List<AppCourse> GetCoursesOfStudent(int studentId);
         void RemoveCourseFromStudent(int studentId, int courseId);
    }
}