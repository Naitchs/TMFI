using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    // [Authorize]
    public class IpProfileController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public IpProfileController(IUserRepository userRepository, IMapper mapper)
        {


            _mapper = mapper;
            _userRepository = userRepository;

        }




        [HttpPost("register-ip")]
        public async Task<ActionResult> RegisterIp(RegisterIpDto registerIpDto)
        {
            try
            {
                // Check if there is an existing record with the same details
                if (await _userRepository.IsDuplicateIpDetailsAsync(registerIpDto))
                {
                    return BadRequest("An existing record with the same details was found. Do you want to proceed?");
                }

                // Generate a new PublicId
                string publicId = _userRepository.IpGeneratePublicId();

                // Use AutoMapper to map RegisterIpDto to AppIp
                var appIp = _mapper.Map<AppIp>(registerIpDto);

                // Set the PublicId property
                appIp.PublicId = publicId;

                // Add the newly created AppIp to your repository or database context
                var addedIp = _userRepository.Add(appIp);

                // Save changes to the database
                if (await _userRepository.SaveAllAsync())
                {
                    // Return a successful response
                    return Ok(addedIp);
                }
                else
                {
                    // Return an error response if saving to the database fails
                    return BadRequest(new { error = "Failed to add IP profiling" });
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during registration, return an error response.
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost("register-ip-proceed")]
        public async Task<ActionResult> ProceedRegisterIp(RegisterIpDto registerIpDto)
        {
            try
            {

                // Generate a new PublicId
                string publicId = _userRepository.IpGeneratePublicId();

                // Use AutoMapper to map RegisterIpDto to AppIp
                var appIp = _mapper.Map<AppIp>(registerIpDto);

                // Set the PublicId property
                appIp.PublicId = publicId;

                // Add the newly created AppIp to your repository or database context
                var addedIp = _userRepository.Add(appIp);

                // Save changes to the database
                if (await _userRepository.SaveAllAsync())
                {
                    // Return a successful response
                    return Ok(addedIp);
                }
                else
                {
                    // Return an error response if saving to the database fails
                    return BadRequest(new { error = "Failed to add IP profiling" });
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during registration, return an error response.
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<IpDto>>> GetIps()
        {

            var ips = await _userRepository.GetIpsAsync();

            return Ok(ips);
        }

        [HttpGet("{publicId}")]
        public async Task<ActionResult<IpDto>> GetIp(string publicId)
        {

            return await _userRepository.GetIpsByIdAsync(publicId);

        }

        [HttpPut("update-ip/{publicId}")]
        public async Task<ActionResult> UpdateIp(string publicId, [FromBody] IpUpdateDto ipUpdateDto)
        {
            try
            {
                var ip = await _userRepository.GetIpsByPublicIdAsync(publicId);

                if (ip == null)
                {
                    return NotFound("User not found");
                }

                _mapper.Map(ipUpdateDto, ip);

                _userRepository.UpdateIp(ip);

                if (await _userRepository.SaveAllAsync())
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Failed to update user");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpDelete("delete-ip/{id}")]
        public IActionResult DeleteSubject(int id)
        {
            _userRepository.DeleteIp(id);

            return Ok();
        }


    }
}