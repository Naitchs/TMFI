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
    public class UsersController : BaseApiController
    {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly ITokenService _tokenService;

    public UsersController(IUserRepository userRepository, IMapper mapper, 
              IPhotoService photoService, ITokenService tokenService){
      _tokenService = tokenService;
     
      _photoService = photoService;
      _mapper = mapper;
      _userRepository = userRepository;
      
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>>GetUsers(){
       
       var users =  await _userRepository.GetMembersAsync();

       return Ok(users);

    }


    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username){
       
        return await _userRepository.GetMemberAsync(username);
  
    }

    // [HttpPut]
    // public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

    //   var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

    //   if (user == null) return NotFound();

    //   _mapper.Map(memberUpdateDto, user);

    //   if (await _userRepository.SaveAllAsync()) return NoContent();

    //   return BadRequest("Failed to update user");

    // }

    [HttpPut]
public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
{
    try
    {
        var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

        if (user == null)
        {
            return NotFound("User not found");
        }

        _mapper.Map(memberUpdateDto, user);

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


    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
        
        var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo{
          Url = result.SecureUrl.AbsoluteUri,
          PublicId = result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync()) {

          return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
          
        }

        return BadRequest("Problem adding photo");

    }

      [HttpPut("set-main-photo/{photoId}")]
      public async Task<ActionResult> SetMainPhoto(int photoId){

          var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

          if (user == null) return NotFound();

          var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

          if (photo == null) return NotFound();

          if (photo.IsMain) return BadRequest("this is already your main photo");

          var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
          if (currentMain != null) currentMain.IsMain = false;
          photo.IsMain = true;

          if (await _userRepository.SaveAllAsync()) return NoContent();

          return BadRequest("Problem setting the main photo");
      }

      [HttpDelete("delete-photo/{photoId}")]
      public async Task<ActionResult> DeletePhoto(int photoId){

          var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

          var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

          if (photo == null) return NotFound();

          if (photo.IsMain) return BadRequest("You Delete your main photo");

          if (photo.PublicId != null){
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
          }
          user.Photos.Remove(photo);
          
          if (await _userRepository.SaveAllAsync()) return Ok();

          return BadRequest("Problem deleting photo");
      }

[HttpPut("change-password")]
public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
{
    try
    {
        var user = await _userRepository.GetUsersByUsernameAsync(User.GetUsername());

        if (user == null)
        {
            return NotFound("User not found");
        }

        // Validate the current password
        if (!await _userRepository.VerifyPasswordAsync(user.UserName, changePasswordDto.CurrentPassword))
        {
            return BadRequest("Current password is incorrect");
        }

        if (await _userRepository.VerifyPasswordAsync(user.UserName, changePasswordDto.NewPassword))
        {
            return BadRequest("New password cannot be the same as the current password");
        }

        // Change the password
        await _userRepository.ChangePasswordAsync(user, changePasswordDto.NewPassword);

        // Create a new token after changing the password
        var newToken = await _tokenService.CreateToken(user);

        // return Ok(new { message = "Change Password successfully", newToken });
        return Ok(new { message = "Change Password successfully" });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}



    }
    
}