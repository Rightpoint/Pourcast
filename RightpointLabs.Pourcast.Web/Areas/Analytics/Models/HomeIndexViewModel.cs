using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RightpointLabs.Pourcast.Application.Payloads.Analytics;

namespace RightpointLabs.Pourcast.Web.Areas.Analytics.Models
{
    public class HomeIndexViewModel
    {
        [Display(Name = "Beers We've Had")]
        public IEnumerable<BeerBeenOnTap> BeersBeenOnTap { get; set; } 
    }
}