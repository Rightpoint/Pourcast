using RightpointLabs.Pourcast.Web.Properties;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class CreateTapViewModel
    {
        public CreateTapViewModel() { }

        public CreateTapViewModel(IEnumerable<KegModel> kegs)
        {
            if (null == kegs) throw new ArgumentNullException(nameof(kegs));

            Kegs = kegs.Select(k => new SelectListItem() { Text = k.BeerName, Value = k.Id }).ToList();
            Kegs.Insert(0, new SelectListItem() { Text = Resources.Admin_Tap_Create_Dropdownlist, Value = "", Selected = true});
        }

        [Required]
        public string Name { get; set; }
        public List<SelectListItem> Kegs { get; private set; }
        [Display(Name = "Keg")]
        public string KegId { get; set; }
    }
}