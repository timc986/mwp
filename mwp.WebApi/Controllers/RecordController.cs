﻿using System;
using System.Linq;
using System.Threading.Tasks;
using mwp.DataAccess.Dto;
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

        public RecordController(IRecordService recordService)
        {
            this.recordService = recordService;
        }
        
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRecord([FromBody]RecordDto createRecord)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }
                
                var record = await recordService.CreateRecord(createRecord, userIdClaim.Value);

                return Ok(new { record });
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
                    return BadRequest(new { message = "Invalid token" });
                }

                var records = await recordService.GetUserRecord(userIdClaim.Value);

                return Ok(new { records });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRecord([FromBody]RecordDto updateRecord)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                var record = await recordService.UpdateRecord(updateRecord, userIdClaim.Value);

                return Ok(new { record });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete")] //delete?recordId=999
        public async Task<IActionResult> DeleteRecord(long recordId)
        {
            try
            {
                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                await recordService.DeleteRecord(recordId, userIdClaim.Value);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
