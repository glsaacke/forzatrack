using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;
using api.core.services.RecordService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using Microsoft.Extensions.Logging;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private IRecordService recordService;
        private ILogger<RecordController> logger;
        public RecordController(IRecordService recordService, ILogger<RecordController> logger){
            this.recordService = recordService;
            this.logger = logger;
        }

        [HttpGet("GetAllRecords")]
        public IActionResult GetAllRecords()
        {
            try{
                var records = recordService.GetAllRecords();

                if (records == null || records.Count == 0)
                {
                    logger.LogWarning("No records found.");
                    return NotFound(new { Message = "No records found." });
                } else {
                    return Ok(records);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all records.");
                throw;
            }
        }

        [HttpGet("GetRecordById/{id}")]
        public IActionResult GetRecordById(int id)
        {
            Record record;
            try{
                record = recordService.GetRecordByID(id);

                if (record == null){
                    logger.LogWarning("No cars found.");
                    return NotFound(new { Message = "No cars found." });
                } else {
                    return Ok(record);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching record.");
                throw;
            }
        }

        [HttpPost("CreateRecord")]
        public IActionResult CreateRecord([FromBody] RecordRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Record record;

                try{

                    record = new Record{
                        UserId = request.UserId,
                        CarId = request.CarId,
                        Event = request.Event,
                        ClassRank = request.ClassRank,
                        TimeMin = request.TimeMin,
                        TimeSec = request.TimeSec,
                        TimeMs = request.TimeMs,
                        CpuDiff = request.CpuDiff,
                        Deleted = request.Deleted
                    };

                    recordService.CreateRecord(record);

                    return Ok();
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while creating record.");
                    throw;
                }
            }
        }

        [HttpPut("UpdateRecord/{id}")]
        public IActionResult UpdateRecord(int id, [FromBody] RecordRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Record record;

                try{

                    record = new Record{
                        UserId = request.UserId,
                        CarId = request.CarId,
                        Event = request.Event,
                        ClassRank = request.ClassRank,
                        TimeMin = request.TimeMin,
                        TimeSec = request.TimeSec,
                        TimeMs = request.TimeMs,
                        CpuDiff = request.CpuDiff,
                        Deleted = request.Deleted
                    };

                    bool rowsAffected = recordService.UpdateRecord(record, id);
                    if(rowsAffected){
                        return Ok();
                    } else {
                        return NotFound("No Records found matching the id.");
                    }
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while updating record.");
                    throw;
                }
            }
        }

        [HttpPut("SetRecordDeleted/{id}")]
        public IActionResult SetRecordDeleted(int id)
        {
            try{
                bool rowsAffected = recordService.SetRecordDeleted(id);
                if(rowsAffected){
                    return Ok();
                } else {
                    return NotFound("No Records found matching the id.");
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occured while setting Record Deleted");
                throw;
            }
        }

        [HttpDelete("DeleteRecord/{id}")]
        public IActionResult DeleteRecord(int id)
        {
             try{
                recordService.DeleteRecord(id);
                return Ok();
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while deleting record.");
                throw;
            }
        }

        [HttpGet("GetRecordsByUserId")]
        public IActionResult GetRecordsByUserId(int id)
        {
            try{
                var records = recordService.GetRecordsByUserId(id);

                return Ok(records);
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching records.");
                throw;
            }
        }
    }
}