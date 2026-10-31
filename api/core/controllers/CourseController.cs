using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.services.CourseService;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet("GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            try
            {
                var courses = _courseService.GetAllCourses();
                if (courses == null || courses.Count == 0)
                {
                    _logger.LogWarning("No courses found.");
                    return NotFound(new { Message = "No courses found." });
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all courses.");
                throw;
            }
        }

        [HttpGet("GetCourseById/{id}")]
        public IActionResult GetCourseById(int id)
        {
            try
            {
                var course = _courseService.GetCourseById(id);
                if (course == null)
                {
                    _logger.LogWarning("No course found with id {Id}.", id);
                    return NotFound(new { Message = "No course found." });
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching course.");
                throw;
            }
        }

        [HttpGet("GetCoursesByGame")]
        public IActionResult GetCoursesByGame([FromQuery] string game)
        {
            try
            {
                var courses = _courseService.GetCoursesByGame(game);
                if (courses == null || courses.Count == 0)
                {
                    _logger.LogWarning("No courses found for game {Game}.", game);
                    return NotFound(new { Message = "No courses found." });
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching courses by game.");
                throw;
            }
        }

        [HttpPost("CreateCourse")]
        public IActionResult CreateCourse([FromBody] CourseRequest request)
        {
            try
            {
                var course = new Course
                {
                    Name = request.Name,
                    Game = request.Game,
                    Deleted = 0
                };
                _courseService.CreateCourse(course);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating course.");
                throw;
            }
        }

        [HttpPut("UpdateCourse/{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] CourseRequest request)
        {
            try
            {
                var course = new Course
                {
                    Name = request.Name,
                    Game = request.Game
                };
                bool updated = _courseService.UpdateCourse(course, id);
                if (updated)
                {
                    return Ok();
                }
                return NotFound(new { Message = "No course found matching the id." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating course.");
                throw;
            }
        }

        [HttpPut("SetCourseDeleted/{id}")]
        public IActionResult SetCourseDeleted(int id)
        {
            try
            {
                bool updated = _courseService.SetCourseDeleted(id);
                if (updated)
                {
                    return Ok();
                }
                return NotFound(new { Message = "No course found matching the id." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting course deleted.");
                throw;
            }
        }

        [HttpDelete("DeleteCourse/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            try
            {
                _courseService.DeleteCourse(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting course.");
                throw;
            }
        }
    }
}
