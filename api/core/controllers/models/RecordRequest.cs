using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class RecordRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CarId { get; set; }

        [Required]
        [StringLength(100)]
        public string Event { get; set; }

        [Required]
        [StringLength(10)]
        public string ClassRank { get; set; }

        [Range(0, 59)]
        public int TimeMin { get; set; }

        [Range(0, 59)]
        public int TimeSec { get; set; }

        [Range(0, 999)]
        public int TimeMs { get; set; }

        [Required]
        [StringLength(20)]
        public string CpuDiff { get; set; }

        public int Deleted { get; set; } = 0;
    }
}