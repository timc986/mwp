using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;
using mwp.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mwp.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService recordService;
        private readonly IMapper mapper;

        public RecordController(IRecordService recordService, IMapper mapper)
        {
            this.recordService = recordService;
            this.mapper = mapper;
        }
        
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRecord([FromBody]RecordDto recordDto)
        {
            try
            {
                var currentUser = HttpContext.User;

                var userIdClaim = currentUser.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest();
                }

                var record = mapper.Map<Record>(recordDto);
                record.UserId = Convert.ToInt64(userIdClaim.Value);

                var result = await recordService.CreateRecord(record);
                return Ok(new { recordId = result.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
