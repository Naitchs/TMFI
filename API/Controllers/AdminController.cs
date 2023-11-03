using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
    private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
      _userManager = userManager;
            
        }

        [Authorize(Policy = "RequireSuperAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles(){
            var users = await _userManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();

            return Ok(users);
        }
 
        [Authorize(Policy = "RequireSuperAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles){
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to Add Roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        // public ActionResult GetPhotosForModerations(){
        //     return Ok("super admin or admin can see this");
        // }
//        [Authorize(Policy = "RequireSuperAdminRole")]
// [HttpPost("edit-active-status/{username}")]
// public async Task<ActionResult> EditActiveStatus(string username, [FromBody]string activeStatus)
// {
//     var user = await _userManager.FindByNameAsync(username);

//     if (user == null) return NotFound();


//     user.ActiveStatus = activeStatus; // Assuming the property name is ActiveStatus in your AppUser class

//     var result = await _userManager.UpdateAsync(user);

//     if (result.Succeeded)
//     {
//         return Ok(user.ActiveStatus);
//     }

//     return BadRequest("Failed to update active status");
// }


    }
}