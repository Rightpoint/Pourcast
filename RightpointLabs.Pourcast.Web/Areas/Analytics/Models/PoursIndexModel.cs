using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Models
{
    public class PoursIndexModel
    {
        public List<PourAnalysis> ByDay { get; set; }
        public List<PourAnalysis> ByTime { get; set; }
        public List<PourAnalysis> ByDayAndTime { get; set; }
        public List<PourAnalysis> ByDate { get; set; }
        public List<BeerInfo> LastWeekBeers { get; set; }
        public List<BeerPourAnalysis> ByDayAndTimeAndBeer { get; set; }
        public List<BeerInfo> Beers { get; set; }
        public List<BeerPourAnalysis> ByDateAndBeer { get; set; }
    }
}