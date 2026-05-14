using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class BuildRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CarId { get; set; }

        [Required]
        [Range(1, 999)]
        public int Rank { get; set; }

        [Range(0.0, 10.0)]
        public double SpeedST { get; set; }

        [Range(0.0, 10.0)]
        public double HandlingST { get; set; }

        [Range(0.0, 10.0)]
        public double AccelerationST { get; set; }

        [Range(0.0, 10.0)]
        public double LaunchST  { get; set; }

        [Range(0.0, 10.0)]
        public double BrakingST { get; set; }

        [Range(0.0, 10.0)]
        public double OffroadST { get; set; }

        [Range(0.0, 500.0)]
        public double TopSpeed { get; set; }

        [Range(0.0, 30.0)]
        public double ZeroToSixty { get; set; }

        public int Deleted { get; set; } = 0;
    }
}