using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class CourseRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Game { get; set; }
    }
}
