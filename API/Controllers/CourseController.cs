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

        public CourseController(IMapper mapper, ICourseService courseService
                               )
        {

            _courseService = courseService;
            _mapper = mapper;
        }


        //        [HttpPost("add-course")]
        // public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
        // {
        //     try
        //     {
        //         var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

        //         if (user == null)
        //         {
        //             return NotFound(new { error = "User not found" });
        //         }

        //         var appCourse = _mapper.Map<AppCourse>(courseDto);


        //         // Make sure user.Courses is initialized (not null)
        //         user.Courses ??= new List<AppCourse>();


        //         var facilitatorInfo = await _userRepository.GetFacilitatorInfoAsync(User.GetUsername());

        //         if (facilitatorInfo != null)
        //         {
        //             appCourse.FacilitatorId = facilitatorInfo.Id;
        //             appCourse.FacilitatorName = facilitatorInfo.UserName;
        //         }

        //         user.Courses.Add(appCourse);

        //         if (await _userRepository.SaveAllAsync())
        //         {
        //             return Ok(new { message = "Course added successfully" });
        //         }
        //         else
        //         {
        //             return BadRequest(new { error = "Failed to add Course" });
        //         }
        //     }
        //     catch (Exception ex)
        //     {

        //        if (ex.InnerException != null)
        //     {
        //         Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        //     }

        //     return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        //     }
        // }

        // [HttpPost("enroll-students")]
        // public async Task<IActionResult> EnrollStudents([FromBody] EnrollmentDto enrollmentDto)
        // {
        //     try
        //     {
        //         var facilitator = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

        //         if (facilitator == null)
        //         {
        //             return NotFound(new { error = "Facilitator not found" });
        //         }

        //         var course = await _userRepository.GetCourseByIdAsync(enrollmentDto.CourseId);

        //         if (course == null)
        //         {
        //             return NotFound(new { error = $"Course with ID {enrollmentDto.CourseId} not found" });
        //         }

        //         foreach (var studentDto in enrollmentDto.Students)
        //         {
        //             var studentInfo = await _userRepository.GetStudentInfoAsync(studentDto.Id);

        //             if (studentInfo == null)
        //             {
        //                 return NotFound(new { error = $"Student with ID {studentDto.Id} not found" });
        //             }

        //             if (course.Enrollments.Any(e => e.StudentEnrollments.Any(se => se.StudentId == studentDto.Id)))
        //             {
        //                 return BadRequest(new { error = $"Student with ID {studentDto.Id} is already enrolled in the course" });
        //             }

        //             var appStudentEnrollment = new AppStudentEnrollment
        //             {
        //                 StudentId = studentDto.Id,
        //                 StudentName = $"{studentDto.Firstname} {studentDto.Lastname}",
        //                 EnrollmentId = enrollmentDto.EnrollmentId,
        //                 Enrollment = null
        //             };

        //             studentInfo.Enrollments ??= new List<AppStudentEnrollment>();
        //             studentInfo.Enrollments.Add(appStudentEnrollment);
        //         }

        //         if (await _userRepository.SaveAllAsync())
        //         {
        //             return Ok(new { message = "Students enrolled successfully" });
        //         }
        //         else
        //         {
        //             return BadRequest(new { error = "Failed to enroll students" });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
        //     }
        // }





        //    [HttpGet]
        //    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(){

        //     var courses = await _userRepository.GetCoursesAsync();

        //     return Ok(courses);


        //    }

        //     [HttpGet("facilitators")]
        //     public async Task<ActionResult<IEnumerable<FacilitatorDto>>>GetFacilitators(){

        //        var users =  await _userRepository.GetFacilitatorsAsync();

        //        return Ok(users);

        //     }

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
                return Ok("Subjects added to course successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("get-subjects-in-course/{courseId}")]
        public IActionResult GetSubjectsInCourse(int courseId)
        {
            try
            {
                var subjectsInCourse = _courseService.GetSubjectsInCourse(courseId);

                // Use JsonSerializerOptions to configure reference handling
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 32  // You can adjust this depth based on your needs
                };

                var json = JsonSerializer.Serialize(subjectsInCourse, options);

                // Return JSON result
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
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
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpDelete("remove-subjects-from-course")]
        public IActionResult RemoveSubjectsFromCourse(RemoveSubjectsFromCourseDto dto)
        {
            try
            {
                _courseService.RemoveSubjectsFromCourse(dto.CourseId, dto.SubjectIds);
                return Ok("Subjects removed from course successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        // [HttpPost("add-subjects-to-course")]
        // public IActionResult AddSubjectsToCourse([FromBody] AddSubjectsToCourseDto addSubjectsToCourse)
        // {
        //     var course = _courseService.GetCourseId(addSubjectsToCourse.CourseId);

        //     if (course == null)
        //     {
        //         return NotFound("Course not found");
        //     }
        //     if (addSubjectsToCourse.SubjectIds != null)
        //     {
        //         foreach (var subjectId in addSubjectsToCourse.SubjectIds)
        //         {
        //             var subject = _courseService.GetSubjectById(subjectId);

        //             if (subject == null)
        //             {
        //                 return NotFound($"Subject with ID {subjectId} not found");
        //             }

        //             _courseService.AddSubjectToCourse(addSubjectsToCourse.CourseId, subjectId);
        //         }
        //     }
        //     return Ok("Subjects added to course successfully");
        // }


    }
}