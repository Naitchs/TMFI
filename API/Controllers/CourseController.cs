using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{

    public class CourseController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;
        private readonly LogService _logService;

        public CourseController(IMapper mapper, ICourseService courseService, LogService logService
                               )
        {
            _logService = logService;
            _courseService = courseService;
            _mapper = mapper;
        }


        [HttpPost("add-course")]
        public IActionResult CreateCourse(CourseDto courseDto)
        {

            var appCourse = _mapper.Map<AppCourse>(courseDto);

            var addedCourse = _courseService.AddCourse(appCourse);

            return Ok(addedCourse);

        }

        [HttpPut("edit-course/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto courseDto)
        {
            var course = await _courseService.GetCourseId(id);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            _mapper.Map(courseDto, course);


            _courseService.UpdateCourse(course);

            return NoContent();

        }


        [HttpGet("get-courses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {

            var course = await _courseService.GetCoursesAsync();

            return Ok(course);
        }

        [HttpGet("get-course/{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {

            var course = await _courseService.GetCourseId(id);

            return Ok(course);
        }

        [HttpDelete("delete-course/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);

            return Ok(_courseService.GetCoursesAsync());
        }

        [HttpPost("add-subject")]
        public IActionResult CreateSubject(SubjectDto subjectDto)
        {

            var appSubject = _mapper.Map<AppSubject>(subjectDto);
            var addedSubject = _courseService.AddSubject(appSubject);

            return Ok(addedSubject);

        }

        [HttpPut("edit-subject/{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectDto subjectDto)
        {
            var subject = await _courseService.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound("Course not found");
            }

            _mapper.Map(subjectDto, subject);


            _courseService.UpdateSubject(subject);

            return NoContent();

        }

        [HttpGet("get-subjects")]
        public async Task<ActionResult<IEnumerable<IpDto>>> GetSubjects()
        {

            var course = await _courseService.GetSubjectsAsync();

            return Ok(course);
        }

        [HttpGet("get-subject/{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {

            var subject = await _courseService.GetSubjectId(id);

            return Ok(subject);
        }

        [HttpDelete("delete-subject/{id}")]
        public IActionResult DeleteSubject(int id)
        {
            _courseService.DeleteSubject(id);

            return Ok();
        }

        [HttpPost("add-subjects-to-course")]
        public IActionResult AddSubjectsToTheCourse(AddSubjectsToCourseDto dto)
        {
            try
            {
                _courseService.AddSubjectsToCourse(dto.CourseId, dto.SubjectIds);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        // [HttpGet("get-subjects-in-course/{courseId}")]
        // public IActionResult GetSubjectsInCourse(int courseId)
        // {
        //     try
        //     {
        //         var subjectsInCourse = _courseService.GetSubjectsInCourse(courseId);

        //         // Use JsonSerializerOptions to configure reference handling
        //         var options = new JsonSerializerOptions
        //         {
        //             ReferenceHandler = ReferenceHandler.Preserve,
        //             MaxDepth = 32  // You can adjust this depth based on your needs
        //         };

        //         var json = JsonSerializer.Serialize(subjectsInCourse, options);

        //         // Return JSON result
        //         return Content(json, "application/json");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest($"An error occurred: {ex.Message}");
        //     }

        // }

        [HttpGet("get-subjects-in-course/{courseId}")]
        public IActionResult GetSubjectsInCourse(int courseId)
        {
            try
            {
                var subjectsInCourse = _courseService.GetSubjectsInCourse(courseId);
                return Ok(subjectsInCourse);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-subjects-not-in-course/{courseId}")]
        public IActionResult GetSubjectsNotInCourse(int courseId)
        {
            try
            {
                var subjectsNotInCourse = _courseService.GetSubjectsNotInCourse(courseId);
                return Ok(subjectsNotInCourse);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("update-add-subjects-to-course")]
        public IActionResult AddSubjectsToCourse(AddSubjectsToCourseDto dto)
        {
            try
            {
                _courseService.UpdateAddSubjectsToCourse(dto.CourseId, dto.SubjectIds);
                return Ok("Subjects added to course successfully.");
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpDelete("remove-subjects-from-course")]
        public IActionResult RemoveSubjectsFromCourse(RemoveSubjectsFromCourseDto dto)
        {
            try
            {
                _courseService.RemoveSubjectFromCourse(dto.CourseId, dto.SubjectId);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("add-students-to-course")]
        public IActionResult AddStudentsToCourse(AddStudentsToCourseDto dto)
        {
            try
            {
                _courseService.AddStudentsToCourse(dto.CourseId, dto.StudentIds);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("add-student-to-course")]
        public IActionResult AddStudentToCourse(AddStudentToCourseDto dto)
        {
            try
            {
                _courseService.AddStudentToCourse(dto.CourseId, dto.StudentId);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-students-not-in-course/{courseId}")]
        public IActionResult GetStudentsNotInCourse(int courseId)
        {
            try
            {
                var studentsNotInCourse = _courseService.GetStudentsNotInCourse(courseId);
                return Ok(studentsNotInCourse);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("search-students-not-in-course")]
        public IActionResult SearchStudentsNotInCourse(int courseId, string search = null)
        {
            try
            {
                var studentsNotInCourse = _courseService.SearchStudentsNotInCourse(courseId, search);

                if (studentsNotInCourse.Any())
                {
                    return Ok(studentsNotInCourse);
                }
                else
                {
                    return BadRequest("No students found, Check if it is already Enrolled.");
                }
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("get-students-in-course/{courseId}")]
        public IActionResult GetStudentsInCourse(int courseId)
        {
            try
            {
                var studentsInCourse = _courseService.GetStudentsInCourse(courseId);
                return Ok(studentsInCourse);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("remove-student-from-course")]
        public IActionResult RemoveStudentFromCourse(RemoveStudentFromCourseDto dto)
        {
            try
            {
                _courseService.RemoveStudentFromCourse(dto.CourseId, dto.StudentId);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-courses-of-student/{studentId}")]
        public IActionResult GetCoursesOfStudent(int studentId)
        {
            try
            {
                var coursesOfStudent = _courseService.GetCoursesOfStudent(studentId);
                return Ok(coursesOfStudent);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("remove-course-from-student")]
        public IActionResult RemoveCourseFromStudent(RemoveCourseFromStudentDto dto)
        {
            try
            {
                _courseService.RemoveCourseFromStudent(dto.StudentId, dto.CourseId);
                return Ok("Course removed from student successfully.");
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-attendance")]
        public IActionResult CreateAttendance([FromBody] CreateAttendanceDto dto)
        {
            try
            {
                _courseService.CreateAttendance(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create-multiple-attendances")]
        public IActionResult CreateMultipleAttendances([FromBody] List<CreateAttendanceDto> attendanceDtos)
        {
            try
            {
                foreach (var dto in attendanceDtos)
                {
                    _courseService.CreateAttendance(dto);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("get-grouped-attendance-records/{subjectId}")]
        public IActionResult GetGroupedAttendanceRecords(int subjectId)
        {
            try
            {
                var groupedAttendanceRecords = _courseService.GetAttendanceRecordsGroupedByDate(subjectId);
                return Ok(groupedAttendanceRecords);
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("edit-attendance/{attendanceId}")]
        public IActionResult EditAttendance(int attendanceId, [FromBody] EditAttendanceDto dto)
        {
            try
            {
                _courseService.EditAttendance(attendanceId, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete-attendance/{attendanceId}")]
        public IActionResult DeleteAttendance(int attendanceId)
        {
            try
            {
                _courseService.DeleteAttendance(attendanceId);
                return Ok();
            }
            catch (Exception ex)
            {
                 _logService.AddErrorLogs(ex.ToString());
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }






    }
}