using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mwp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        public ValuesController()
        {
        }

        // GET api/values
        [HttpGet("allvalues")]
        [Authorize]
        public ActionResult<IEnumerable<string>> GetValues()
        {
            var currentUser = HttpContext.User;

            //read claim (Username) from the token
            if (currentUser.HasClaim(c => c.Type == "Username"))
            {
                var xx = currentUser.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
