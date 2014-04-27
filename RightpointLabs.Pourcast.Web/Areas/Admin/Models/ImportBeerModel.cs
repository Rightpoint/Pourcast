using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class ImportBeerModel
    {
        public string Name { get; set; }
        public string Style { get; set; }
        public double? ABV { get; set; }
        public double? BAScore { get; set; }
    }
}