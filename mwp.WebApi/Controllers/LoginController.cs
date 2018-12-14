using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using mwp.DataAccess.Entities;
using mwp.Service.Login;
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
        private readonly ILoginService loginService;

        public LoginController(IConfiguration config, ILoginService loginService)
        {
            this.config = config;
            this.loginService = loginService;
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
        public async Task<IActionResult> Test([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            //var user = AuthenticateUser(login);

            //if (user != null)
            //{
            //    var tokenString = GenerateJsonWebToken(user);
            //    response = Ok(new { token = tokenString });
            //}

            var result = await loginService.CheckUserExist(1);

            var result2 = await loginService.GetUser(1);

            var newUser = new User
            {
                Name = "Tim",
                Email = "timothychan92test@yahoo.com.hk",
                Password = "1234567890",
                UserGroupId = 1,
                UserRoleId = 1,
                UserRole = null,
                UserGroup = null
            };

            var result3 = await loginService.CreateUser(newUser);

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
