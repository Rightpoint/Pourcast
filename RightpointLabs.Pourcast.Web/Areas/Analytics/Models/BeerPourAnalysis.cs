using System;
using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Models
{
    public class BeerPourAnalysis
    {
        public DateTime Date { get; set; }
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public IEnumerable<BeerPourAnalysisItem> Beers { get; set; }
    }
}