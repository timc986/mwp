using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;
using mwp.Service;
using mwp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace mwp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public LoginController(IConfiguration config, IUserService userService, IMapper mapper)
        {
            this.config = config;
            this.userService = userService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserDto login)
        {
            IActionResult response = Unauthorized();

            var user = await userService.Login(login.Name, login.Password);

            if (user == null)
            {
                return Unauthorized(new { error = "Username or password is incorrect" });
            }

            var tokenString = GenerateJsonWebToken(login.Name);
            response = Ok(new { token = tokenString });

            return response;
        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);

            try
            {
                var result = await userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GenerateJsonWebToken(string userName)
        {
            //TODO: store in database?
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //stored username in the token claims
            var claims = new[] {
                new Claim("Username", userName)
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
