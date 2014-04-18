using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class KegsOnTap
    {
        public OnTapKeg RightKeg { get; set; }
        public OnTapKeg LeftKeg { get; set; }
    }
}