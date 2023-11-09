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
    }
}