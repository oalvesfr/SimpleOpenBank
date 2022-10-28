using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleOpenBank.Application.Contracts.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleOpenBank.Auth.Service
{
    public class AuthToken : IAuthToken
    {
        private readonly IConfiguration _configuration;
        public AuthToken(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public Task<string> CreateToken(string uid, DateTime expire)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim("uid", uid));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expire,
                signingCredentials: credentials
                );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public Task<string> CreateRefreshToken(DateTime expire)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                expires: expire,
                signingCredentials: credentials
                );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
