using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper,
        LogService logService)
        {

            _userManager = userManager;
            _mapper = mapper;
            _logService = logService;
            _tokenService = tokenService;

        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            var normalizedUsername = registerDto.Username.ToLower();

            if (await UserExist(normalizedUsername)) return BadRequest("Username is Taken");
            // using var hmac = new HMACSHA512();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            // user.PasswordHalt = hmac.Key;
            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = normalizedUsername;

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            _logService.AddLoginLogs(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var normalizedUsername = loginDto.Username.ToLower();
            var user = await _userManager.Users
               .Include(p => p.Photos)
               .SingleOrDefaultAsync(x =>
               x.UserName == normalizedUsername);

            if (user == null) return Unauthorized("invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            _logService.AddLoginLogs(user);

            if (!result) return Unauthorized("Invalid Password");

            // User is authenticated, generate JWT token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            // Add other claims if needed
            };

            // using var hmac = new HMACSHA512(user.PasswordHalt);

            // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // for (int i = 0; i < computedHash.Length; i++){
            //     if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            // }

            return new UserDto
            {

                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        public async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}