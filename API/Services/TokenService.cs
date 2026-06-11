using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public async Task<string> CreateToken(AppUser user)
        {
            var TokenKey = config["TokenKey"] ?? throw new Exception("Cannot find TokenKey in config");
            if(TokenKey.Length < 64) throw new Exception("TokenKey must be at least 64 characters long");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(15),
                SigningCredentials = creds
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
        
    }
}