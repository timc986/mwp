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

            var tokenString = tokenGenerator.GenerateToken(user.Id.ToString());
            response = Ok(new { token = tokenString });

            return response;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);

            try
            {
                var result = await userService.Create(user, userDto.Password);
                return Ok(new { userId = result.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
    }
}
