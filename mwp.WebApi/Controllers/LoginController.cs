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
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJsonWebToken(user);
                response = Ok(new { token = tokenString });
            }

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

        [AllowAnonymous]
        [Route("test")]
        public async Task<IActionResult> Test([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            //var user = AuthenticateUser(login);

            //if (user != null)
            //{
            //    var tokenString = GenerateJsonWebToken(user);
            //    response = Ok(new { token = tokenString });
            //}

            var existingUser = await userService.GetUser(100);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    Name = "Tim",
                    Email = "timothychan92test@yahoo.com.hk",
                    UserGroupId = 1,
                    UserRoleId = 1
                };

                var serviceResponse = await userService.CreateUser(newUser);

                if (serviceResponse)
                {
                    response = Ok(new { userId = newUser.Id });
                }
            }

            return response;
        }

        private string GenerateJsonWebToken(UserModel userInfo)
        {
            //TODO: store in database?
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //stored username in the token claims
            //information about the user which helps us to authorize the access to a resource.
            var claims = new[] {
                new Claim("Username", userInfo.Username)
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;


            //TODO: check from database
            if (string.Equals(login.Username, "Tim") && string.Equals(login.Password, "Timpw"))
            {
                user = login;
            }

            return user;
        }
    }
}
