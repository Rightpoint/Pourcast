using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class BreweryViewModel
    {
        public Brewery Brewery { get; set; }
        public IEnumerable<Beer> Beers { get; set; } 
    }
}