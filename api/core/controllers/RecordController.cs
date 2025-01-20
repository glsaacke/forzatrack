using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController
    {
        [HttpGet("GetAllRecords")]
        public IEnumerable<string> GetAllRecords()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetRecordById/{id}")]
        public string GetRecordById(int id)
        {
            return "value";
        }

        [HttpPost("CreateRecord")]
        public void CreateRecord([FromBody] string value)
        {
        }

        [HttpPut("UpdateRecord/{id}")]
        public void UpdateRecord(int id, [FromBody] string value)
        {
        }

        [HttpDelete("DeleteRecord/{id}")]
        public void DeleteRecord(int id)
        {
        }
    }
}