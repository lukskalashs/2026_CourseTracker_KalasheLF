using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] // POST api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await userManager.Users.AnyAsync(x => x.UserName == registerDto.Username.ToLower()))
            {
                return BadRequest("Username is already taken");
            }

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await SetRefreshTokenCookie(user);

            return new UserDto
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateToken(user)
            };
        }
        
        [HttpPost("login")] // POST api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result)
            {
                return Unauthorized("Invalid password");
            }

            await SetRefreshTokenCookie(user);

            return new UserDto
            {
                Username = user.UserName!,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateToken(user)
            };
        }
        [HttpPost("refresh-token")] // POST api/account/refresh-token   
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(refreshToken == null)
            {
                return Unauthorized("No refresh token provided");
            }

            var user = await userManager.Users
                .FirstOrDefaultAsync(y => y.RefreshToken == refreshToken && y.RefreshTokenExpiry > DateTime.UtcNow);

            if(user == null) return Unauthorized("Invalid or expired refresh token");

            await SetRefreshTokenCookie(user);

            return new UserDto
            {
                Username = user.UserName!,
                DisplayName = user.DisplayName,
                Token = await tokenService.CreateToken(user)
            };

        }

        [Authorize]
        [HttpPost("Logout")] // POST api/account/logout
        public async Task<ActionResult> Logout()
        {
            var username = User.Identity?.Name;
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);


            if(user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await userManager.UpdateAsync(user);
            }

            Response.Cookies.Delete("refreshToken");



            return Ok();
        } 

        private async Task SetRefreshTokenCookie(AppUser user)
        {
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(14);

            await userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(14)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}