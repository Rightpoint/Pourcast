using System;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Models
{
    public class PourAnalysis
    {
        public DateTime Date { get; set; }
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public double TotalVolume { get; set; }
        public int Count { get; set; }
    }
}