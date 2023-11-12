using API.Data.Migrations;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [Authorize]
    public class SapProfileController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public SapProfileController(IUserRepository userRepository, IMapper mapper)
        {

            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpPost("register-sap")]
        public async Task<ActionResult> RegisterSap(RegisterSapDto registerSapDto)
        {

            try
            {
                if (await _userRepository.IsDuplicateSapDetailsAsync(registerSapDto))
                {
                    return BadRequest("An existing record with the same details was found. Do you want to proceed?");
                }

                string publicId = _userRepository.SapGeneratePublicId();

                // Use AutoMapper to map RegisterIpDto to AppIp
                var appSap = _mapper.Map<AppSap>(registerSapDto);

                appSap.PublicId = publicId;

                // You may want to set additional properties of appIp here if needed.

                // Add the newly created AppIp to your repository or database context
                var addedSap = _userRepository.Add(appSap);

                // Save changes to the database
                if (await _userRepository.SaveAllAsync())
                {
                    // Return a successful response
                    return Ok(addedSap);
                }
                else
                {
                    // Return an error response if saving to the database fails
                    return BadRequest(new { error = "Failed to add SAP profiling" });
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during registration, return an error response.
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }

        }

        [HttpPost("register-sap-proceed")]
        public async Task<ActionResult> ProceedRegisterSap(RegisterSapDto registerSapDto)
        {

            try
            {
                string publicId = _userRepository.SapGeneratePublicId();

                // Use AutoMapper to map RegisterIpDto to AppIp
                var appSap = _mapper.Map<AppSap>(registerSapDto);

                appSap.PublicId = publicId;

                // You may want to set additional properties of appIp here if needed.

                // Add the newly created AppIp to your repository or database context
                var addedSap = _userRepository.Add(appSap);

                // Save changes to the database
                if (await _userRepository.SaveAllAsync())
                {
                    // Return a successful response
                    return Ok(addedSap);
                }
                else
                {
                    // Return an error response if saving to the database fails
                    return BadRequest(new { error = "Failed to add SAP profiling" });
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during registration, return an error response.
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SapDto>>> GetSaps()
        {

            var saps = await _userRepository.GetSapsAsync();

            return Ok(saps);
        }


        [HttpGet("{publicId}")]
        public async Task<ActionResult<SapDto>> GetSap(string publicId)
        {

            return await _userRepository.GetSapsByIdAsync(publicId);
        }

        [HttpPut("update-sap/{publicId}")]
        public async Task<ActionResult> UpdateSap(string publicId, [FromBody] SapUpdateDto sapUpdateDto)
        {
            try
            {
                var sap = await _userRepository.GetSapByPublicIdAsync(publicId);

                if (sap == null)
                {
                    return NotFound("User not found");
                }

                _mapper.Map(sapUpdateDto, sap);

                _userRepository.UpdateSap(sap);

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

        [HttpDelete("delete-sap/{id}")]
        public IActionResult DeleteSubject(int id)
        {
            _userRepository.DeleteSap(id);

            return Ok();
        }


    }

}