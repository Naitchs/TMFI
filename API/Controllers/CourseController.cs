using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Authorize]
    public class CourseController : BaseApiController
    {
//     private readonly DataContext _context;
//     private readonly IMapper _mapper;
//     private readonly IUserRepository _userRepository;
//          public CourseController(DataContext context, IMapper mapper, 
//                                 IUserRepository userRepository){
      
//             _userRepository = userRepository;
//             _context = context;
//             _mapper = mapper;
//          }

      
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

    
    }
}