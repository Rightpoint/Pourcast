using System.Collections.Generic;
using System.Web.Mvc;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class BeerIndexViewModel
    {
        public List<SelectListItem> Breweries
        {
            get; set; 
        }

        public List<SelectListItem> Beers { get; set; }

        public string BreweryId { get; set; }
        public string BeerId { get; set; }
    }
}