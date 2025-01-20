using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.services.BuildService;
using api.core.controllers.models;
using api.core.models;

namespace api.core.controllers //TODO test build endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController
    {
        private IBuildService buildService;
        public BuildController(IBuildService buildService){
            this.buildService = buildService;
        }

        [HttpGet("GetAllBuilds")]
        public List<Build> GetAllBuilds()
        {
            var builds = buildService.GetAllBuilds();
            return builds;
        }

        [HttpGet("GetBuildById/{id}")]
        public Build GetBuildById(int id)
        {
            Build response = buildService.GetBuildByID(id);
            return response;
        }

        [HttpPost("CreateBuild")]
        public void CreateBuild([FromBody] BuildRequest request)
        {
            Build build = new Build{
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
        }

        [HttpPut("UpdateBuild/{id}")]
        public void UpdateBuild(int id, [FromBody] Build request)
        {
            Build build = new Build{
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

            buildService.UpdateBuild(build);
        }

        [HttpDelete("DeleteBuild/{id}")]
        public void DeleteBuild(int id)
        {
            buildService.DeleteBuild(id);
        }
    }
}