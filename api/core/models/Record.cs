using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.models
{
    public class Record
    {
        public int? RecordId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string Event { get; set; }
        public string ClassRank { get; set; }
        public int TimeMin { get; set; }
        public int TimeSec { get; set; }
        public int TimeMs { get; set; }
        public string CpuDiff { get; set; }
        public DateTime AddDate { get; set; }
        public int Deleted { get; set; }
    }
}