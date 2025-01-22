using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.services.BuildService;
using api.core.controllers.models;
using api.core.models;
using Microsoft.Extensions.Logging;

namespace api.core.controllers //TODO test build endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private IBuildService buildService;
        private ILogger<BuildController> logger;
        public BuildController(IBuildService buildService, ILogger<BuildController> logger){
            this.buildService = buildService;
            this.logger = logger;
        }

        [HttpGet("GetAllBuilds")]
        public IActionResult GetAllBuilds()
        {
            try{
                var builds = buildService.GetAllBuilds();

                if (builds == null || builds.Count == 0)
                {
                    logger.LogWarning("No builds found.");
                    return NotFound(new { Message = "No builds found." });
                } else {
                    return Ok(builds);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all builds.");
                throw;
            }
        }

        [HttpGet("GetBuildById/{id}")]
        public IActionResult GetBuildById(int id)
        {
            Build build;
            try{
                build = buildService.GetBuildByID(id);

                if (build == null){
                    logger.LogWarning("No cars found.");
                    return NotFound(new { Message = "No cars found." });
                } else {
                    return Ok(build);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching car.");
                throw;
            }
        }

        [HttpPost("CreateBuild")]
        public IActionResult CreateBuild([FromBody] BuildRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Build build;
                try{

                    build = new Build{
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
                        Deleted = request.Deleted
                    };

                    buildService.CreateBuild(build);

                    return Ok();
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while creating build.");
                    throw;
                }
            }
        }

        [HttpPut("UpdateBuild/{id}")]
        public IActionResult UpdateBuild(int id, [FromBody] BuildRequest request)
        {
             if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Build build;
                try{

                    build = new Build{
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
                        Deleted = request.Deleted
                    };

                    buildService.UpdateBuild(build, id);

                    return Ok();
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while updating build.");
                    throw;
                }
            }
        }

        [HttpDelete("DeleteBuild/{id}")]
        public IActionResult DeleteBuild(int id)
        {
            try{
                buildService.DeleteBuild(id);
                return Ok();
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while deleting build.");
                throw;
            }
        }
    }
}