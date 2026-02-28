using System;

namespace api.core.models
{
    public class Record
    {
        public int? RecordId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string Event { get; set; } = string.Empty;
        public string ClassRank { get; set; } = string.Empty;
        public int TimeMin { get; set; }
        public int TimeSec { get; set; }
        public int TimeMs { get; set; }
        public string CpuDiff { get; set; } = string.Empty;
        public DateTime AddDate { get; set; }
        public int Deleted { get; set; }
    }
}