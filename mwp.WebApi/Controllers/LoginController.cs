using System;
using System.Threading.Tasks;
using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;
using mwp.Service.Service;
using mwp.WebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mwp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IJsonWebTokenGenerator tokenGenerator;

        public LoginController(IUserService userService, IMapper mapper, IJsonWebTokenGenerator tokenGenerator)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDto login)
        {
            IActionResult response = Unauthorized();

            var user = await userService.Login(login.Name, login.Password);

            if (user == null)
            {
                return Unauthorized(new { error = "Username or password is incorrect" });
            }

            var token = tokenGenerator.GenerateToken(user.Id.ToString());

            if (token != null)
            {
                response = Ok(new { token, user });
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserDto createUser)
        {
            var user = mapper.Map<User>(createUser);

            try
            {
                var result = await userService.Create(user, createUser.Password);

                var userDto = mapper.Map<UserDto>(result);

                return Ok(new { user = userDto});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
    }
}
