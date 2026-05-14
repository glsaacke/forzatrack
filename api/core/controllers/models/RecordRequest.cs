using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.controllers.models
{
    public class RecordRequest
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string Event { get; set; }
        public string ClassRank { get; set; }
        public int TimeMin { get; set; }
        public int TimeSec { get; set; }
        public int TimeMs { get; set; }
        public string CpuDiff { get; set; }
        public int Deleted { get; set; } = 0;
    }
}