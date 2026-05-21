using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class CarRequest
    {
        [Required]
        [StringLength(100)]
        public string Make { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Range(1886, 2100)]
        public int Year { get; set; }

        public int Deleted { get; set; } = 0;
    }
}