using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class TapModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public KegModel Keg { get; set; }
    }
}