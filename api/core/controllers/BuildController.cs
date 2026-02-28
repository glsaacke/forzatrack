using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.services.BuildService;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private readonly IBuildService _buildService;

        public BuildController(IBuildService buildService)
        {
            _buildService = buildService;
        }

        [HttpGet("GetAllBuilds")]
        public async Task<IActionResult> GetAllBuilds()
        {
            var builds = await _buildService.GetAllBuildsAsync();

            if (builds == null || builds.Count == 0)
                return NotFound(new { Message = "No builds found." });

            return Ok(builds);
        }

        [HttpGet("GetBuildById/{id}")]
        public async Task<IActionResult> GetBuildById(int id)
        {
            var build = await _buildService.GetBuildByIdAsync(id);

            if (build == null)
                return NotFound(new { Message = "No build found matching the id." });

            return Ok(build);
        }

        [HttpPost("CreateBuild")]
        public async Task<IActionResult> CreateBuild([FromBody] BuildRequest request)
        {
            var build = new Build
            {
                UserId = request.UserId,
                CarId = request.CarId,
                Rank = request.Rank,
                SpeedST = request.SpeedST,
                HandlingST = request.HandlingST,
                AccelerationST = request.AccelerationST,
                LaunchST = request.LaunchST,
                BrakingST = request.BrakingST,
                OffroadST = request.OffroadST,
                TopSpeed = request.TopSpeed,
                ZeroToSixty = request.ZeroToSixty,
            };

            await _buildService.CreateBuildAsync(build);
            return Ok();
        }

        [HttpPut("UpdateBuild/{id}")]
        public async Task<IActionResult> UpdateBuild(int id, [FromBody] BuildRequest request)
        {
            var build = new Build
            {
                UserId = request.UserId,
                CarId = request.CarId,
                Rank = request.Rank,
                SpeedST = request.SpeedST,
                HandlingST = request.HandlingST,
                AccelerationST = request.AccelerationST,
                LaunchST = request.LaunchST,
                BrakingST = request.BrakingST,
                OffroadST = request.OffroadST,
                TopSpeed = request.TopSpeed,
                ZeroToSixty = request.ZeroToSixty,
            };

            var updated = await _buildService.UpdateBuildAsync(build, id);
            return updated ? Ok() : NotFound("No build found matching the id.");
        }

        [HttpPut("SetBuildDeleted/{id}")]
        public async Task<IActionResult> SetBuildDeleted(int id)
        {
            var updated = await _buildService.SetBuildDeletedAsync(id);
            return updated ? Ok() : NotFound("No build found matching the id.");
        }

        [HttpDelete("DeleteBuild/{id}")]
        public async Task<IActionResult> DeleteBuild(int id)
        {
            await _buildService.DeleteBuildAsync(id);
            return Ok();
        }
    }
}