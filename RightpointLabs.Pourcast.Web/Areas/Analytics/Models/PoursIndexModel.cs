using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Models
{
    public class PoursIndexModel
    {
        public List<PourAnalysis> ByDay { get; set; }
        public List<PourAnalysis> ByTime { get; set; }
        public List<PourAnalysis> ByDayAndTime { get; set; }
        public List<PourAnalysis> ByDate { get; set; }
    }
}