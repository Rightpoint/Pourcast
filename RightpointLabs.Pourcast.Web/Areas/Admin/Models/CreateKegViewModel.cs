using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateType;
using Microsoft.Ajax.Utilities;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateKegViewModel
    {
        public CreateKegViewModel() { }

        public CreateKegViewModel(IEnumerable<Beer> beers)
        {
            if(beers == null) throw new ArgumentNullException("beers");
            Beers = new SelectList(beers.Select(b => new SelectListItem(){Text = b.Name, Value = b.Id}), "Value", "Text");
        }

        public SelectList Beers { get; set; }
        [Display(Name = "Beer")]
        [Required(ErrorMessage = "You must select a beer")]
        public string BeerId { get; set; }
        [Required(ErrorMessage = "Capcity is required")]
        public double Capacity { get; set; }
    }
}