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
        [Required(ErrorMessage = "Capacity is required")]
        public double Capacity { get; set; }
        [Required(ErrorMessage = "Amount of Beer Poured is required")]
        [Display(Name = "Amount of Beer Poured")]
        public double AmountOfBeerPoured { get; set; }

        [Display(Name = "Percent of Beer Remaining")]
        public double PercentOfBeerRemaining { get; set; }
    }
}