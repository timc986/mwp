using System;
using System.Collections.Generic;
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
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
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

        [Authorize]
        [HttpGet("getByUser")]
        public async Task<IActionResult> GetRecord()
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest();
                }

                var userId = Convert.ToInt64(userIdClaim.Value);

                var records = await recordService.GetUserRecord(userId);
                var recordDtos = mapper.Map<List<RecordDto>>(records);

                return Ok(new { recordList = recordDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
