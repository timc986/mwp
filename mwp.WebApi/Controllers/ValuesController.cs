using System.Collections.Generic;
using System.Linq;
using mwp.WebApi.Models;
using mwp.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mwp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IInventoryService services;

        public ValuesController(IInventoryService services)
        {
            this.services = services;
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

        // POST api/values
        [HttpPost]
        [Route("AddInventoryItems")]
        public ActionResult<InventoryItems> AddInventoryItems(InventoryItems items)
        {
            var inventoryItems = services.AddInventoryItems(items);

            if (inventoryItems == null)
            {
                return NotFound();
            }
            return inventoryItems;
        }

        [HttpGet]
        [Route("GetInventoryItems")]
        public ActionResult<Dictionary<string, InventoryItems>> GetInventoryItems()
        {
            var inventoryItems = services.GetInventoryItems();

            if (inventoryItems.Count == 0)
            {
                return NotFound();
            }
            return inventoryItems;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
