using API.Data;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICourseService
    {

        void AddCourse(AppCourse course);

        void UpdateCourse(AppCourse course);

        void DeleteCourse(int id);

        Task<AppCourse> GetCourseById(int id);

        Task<IEnumerable<AppCourse>> GetCoursesAsync();
        Task<IEnumerable<AppSubject>> GetSubjectsAsync();
        Task<AppCourse> GetCourseByIdAsync();


        void AddSubject(AppSubject subject);
        void UpdateSubject(AppSubject subject);
        void DeleteSubject(int id);
        Task<AppSubject> GetSubjectById(int id);
    }
}