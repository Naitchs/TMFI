using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class IpProfileController : BaseApiController
    {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
        public IpProfileController (IUserRepository userRepository,  IMapper mapper ){
            
            _mapper = mapper;
            _userRepository = userRepository;

        }

        
   [HttpPost("register-ip")]
   public async Task<ActionResult> RegisterIp(RegisterIpDto registerIpDto)
   {
      try
      {
        // Use AutoMapper to map RegisterIpDto to AppIp
        var appIp = _mapper.Map<AppIp>(registerIpDto);

        // You may want to set additional properties of appIp here if needed.

        // Add the newly created AppIp to your repository or database context
        _userRepository.Add(appIp);

        // Save changes to the database
        if (await _userRepository.SaveAllAsync())
        {
            // Return a successful response
             return Ok(new { message = "IP profiling added successfully" });
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
   public async Task<ActionResult<IEnumerable<IpDto>>> GetIps(){

    var ips = await _userRepository.GetIpsAsync();

    return Ok(ips);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<IpDto>> GetIp(int id){

    return await _userRepository.GetIpsByIdAsync(id);
    
   }

    }




}