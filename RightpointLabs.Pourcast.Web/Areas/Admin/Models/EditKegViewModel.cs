using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class EditKegViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Beer")]
        public string BeerName { get; set; }
        [Display(Name = "Empty")]
        public bool IsEmpty { get; set; }
    }
}