using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.models
{
    public class Build
    {
        public int? BuildId { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public int Rank { get; set; }
        public double SpeedST { get; set; }
        public double HandlingST { get; set; }
        public double AccelerationST { get; set; }
        public double LaunchST  { get; set; }
        public double BrakingST { get; set; }
        public double OffroadST { get; set; }
        public double TopSpeed { get; set; }
        public double ZeroToSixty { get; set; }
        public int Deleted { get; set; }
    }
}