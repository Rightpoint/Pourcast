using System.ComponentModel.DataAnnotations;
using RightpointLabs.Pourcast.Web.Properties;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{

    using System.Linq;
    using System;
    using WebGrease.Css.Extensions;
    using System.Collections.Generic;
    using RightpointLabs.Pourcast.Domain.Models;
    using System.Web.Mvc;

    public class EditTapViewModel
    {
        public EditTapViewModel() { }

        public EditTapViewModel(IEnumerable<KegModel> kegs, string kegId)
        {
            if(null == kegs) throw new ArgumentNullException(nameof(kegs));

            Kegs = kegs.Select(k => new SelectListItem() {Text = k.BeerName, Value = k.Id, Selected = (k.Id == kegId)}).ToList();
            Kegs.Insert(0, new SelectListItem() {Text = Resources.Admin_Tap_Edit_Dropdownlist, Value = ""});
        }

        public string Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Keg")]
        public string KegId { get; set; }
        public List<SelectListItem> Kegs { get; private set; }

    }
}