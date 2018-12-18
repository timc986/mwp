using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace mwp.WebApi.Helper
{
    public class JsonWebTokenGenerator : IJsonWebTokenGenerator
    {
        private readonly IConfiguration config;

        public JsonWebTokenGenerator(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(string userId)
        {
            //TODO: store in database?
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //stored username in the token claims
            var claims = new[] {
                new Claim("userId", userId)
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
