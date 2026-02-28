using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.models.responses;
using api.core.services.RecordService;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("GetAllRecords")]
        public async Task<IActionResult> GetAllRecords()
        {
            var records = await _recordService.GetAllRecordsAsync();

            if (records == null || records.Count == 0)
                return NotFound(new { Message = "No records found." });

            return Ok(records);
        }

        [HttpGet("GetRecordById/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var record = await _recordService.GetRecordByIdAsync(id);

            if (record == null)
                return NotFound(new { Message = "No record found matching the id." });

            return Ok(record);
        }

        [HttpPost("CreateRecord")]
        public async Task<IActionResult> CreateRecord([FromBody] RecordRequest request)
        {
            if (request.TimeMin.ToString().Length > 2
                || request.TimeSec.ToString().Length > 2
                || request.TimeMs.ToString().Length > 3)
            {
                return Ok(new DefaultResponse { Success = false, Message = "Error: please enter a valid time" });
            }

            var record = new Record
            {
                UserId = request.UserId,
                CarId = request.CarId,
                Event = request.Event,
                ClassRank = request.ClassRank,
                TimeMin = request.TimeMin,
                TimeSec = request.TimeSec,
                TimeMs = request.TimeMs,
                CpuDiff = request.CpuDiff,
                AddDate = DateTime.Today,
            };

            await _recordService.CreateRecordAsync(record);
            return Ok(new DefaultResponse { Success = true, Message = "" });
        }

        [HttpPut("UpdateRecord/{id}")]
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] RecordRequest request)
        {
            var record = new Record
            {
                UserId = request.UserId,
                CarId = request.CarId,
                Event = request.Event,
                ClassRank = request.ClassRank,
                TimeMin = request.TimeMin,
                TimeSec = request.TimeSec,
                TimeMs = request.TimeMs,
                CpuDiff = request.CpuDiff,
            };

            var updated = await _recordService.UpdateRecordAsync(record, id);
            return updated ? Ok() : NotFound("No record found matching the id.");
        }

        [HttpPut("SetRecordDeleted/{id}")]
        public async Task<IActionResult> SetRecordDeleted(int id)
        {
            var updated = await _recordService.SetRecordDeletedAsync(id);
            return updated ? Ok() : NotFound("No record found matching the id.");
        }

        [HttpDelete("DeleteRecord/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            await _recordService.DeleteRecordAsync(id);
            return Ok();
        }

        [HttpGet("GetRecordsByUserId")]
        public async Task<IActionResult> GetRecordsByUserId(int id)
        {
            var records = await _recordService.GetRecordsByUserIdAsync(id);
            return Ok(records);
        }
    }
}