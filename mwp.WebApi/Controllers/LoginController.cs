using System;
using System.Linq;
using System.Threading.Tasks;
using mwp.DataAccess.Dto;
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
        private readonly IJsonWebTokenGenerator tokenGenerator;

        public LoginController(IUserService userService, IJsonWebTokenGenerator tokenGenerator)
        {
            this.userService = userService;
            this.tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserDto login)
        {
            try
            {
                var user = await userService.Login(login.Name, login.Password);

                var token = tokenGenerator.GenerateToken(user.Id.ToString());

                return Ok(new { token, user });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserDto createUser)
        {
            try
            {
                var user = await userService.Create(createUser);

                return Ok(new { user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUsernameEmail([FromBody]UserDto updateUser)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                var user = await userService.UpdateUsernameEmail(updateUser, userIdClaim.Value);

                return Ok(new { user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
