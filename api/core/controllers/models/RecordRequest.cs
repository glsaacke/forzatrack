namespace api.core.controllers.models
{
    public class RecordRequest
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string Event { get; set; } = string.Empty;
        public string ClassRank { get; set; } = string.Empty;
        public int TimeMin { get; set; }
        public int TimeSec { get; set; }
        public int TimeMs { get; set; }
        public string CpuDiff { get; set; } = string.Empty;
    }
}