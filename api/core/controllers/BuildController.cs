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
    public class BuildController
    {
        [HttpGet("GetAllBuilds")]
        public IEnumerable<string> GetAllBuilds()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetBuildById/{id}")]
        public string GetBuildById(int id)
        {
            return "value";
        }

        [HttpPost("CreateBuild")]
        public void CreateBuild([FromBody] string value)
        {
        }

        [HttpPut("UpdateBuild/{id}")]
        public void UpdateBuild(int id, [FromBody] string value)
        {
        }

        [HttpDelete("DeleteBuild/{id}")]
        public void DeleteBuild(int id)
        {
        }
    }
}