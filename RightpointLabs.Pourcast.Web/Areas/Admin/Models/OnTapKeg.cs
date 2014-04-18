using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class OnTapKeg
    {
        public Tap Tap { get; set; }
        public Keg Keg { get; set; }
        public Beer Beer { get; set; }
    }
}