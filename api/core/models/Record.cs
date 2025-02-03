using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;

namespace api.core.models
{
    public class Record
    {
        public int? RecordId { get; set; }
        public int UserId { get; set; }
        public int BuildId { get; set; }
        public string Event { get; set; }
        public string ClassRank { get; set; }
        public int TimeMin { get; set; }
        public int TimeSec { get; set; }
        public int TimeMs { get; set; }
        public string CpuDiff { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public int Deleted { get; set; }
    }
}