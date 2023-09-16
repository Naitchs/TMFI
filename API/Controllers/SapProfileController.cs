using API.Data.Migrations;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SapProfileController : BaseApiController
    {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
        public SapProfileController(IUserRepository userRepository, IMapper mapper){
            
            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpPost("register-sap")]
        public async Task<ActionResult> RegisterSap(RegisterSapDto registerSapDto){
            
            try
      {
        // Use AutoMapper to map RegisterIpDto to AppIp
        var appSap = _mapper.Map<AppSap>(registerSapDto);

        // You may want to set additional properties of appIp here if needed.

        // Add the newly created AppIp to your repository or database context
        _userRepository.Add(appSap);

        // Save changes to the database
        if (await _userRepository.SaveAllAsync())
        {
            // Return a successful response
             return Ok(new { message = "SAP profiling added successfully" });
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
    public async Task<ActionResult<IEnumerable<SapDto>>> GetSaps(){

        var saps = await _userRepository.GetSapsAsync();

        return Ok(saps);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<SapDto>> GetSap(int id){

        return await _userRepository.GetSapsByIdAsync(id);
    }
    }
}