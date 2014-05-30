using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class EditBeerViewModel
    {
        public EditBeerViewModel() { }

        public EditBeerViewModel(IEnumerable<Style> styles)
        {
            Styles = styles.Select(s => new SelectListItem() {Text = s.Name, Value = s.Id});
        }

        public string Id { get; set; }
        [Required(ErrorMessage = "Beer name is required.")]
        public string Name { get; set; }
        public double ABV { get; set; }
        [DisplayName("Beer Advocate Score")]
        public double BAScore { get; set; }
        public IEnumerable<SelectListItem> Styles { get; set; }
        [DisplayName("Style")]
        [Required(ErrorMessage = "You must select a style")]
        public string StyleId { get; set; }
        public string BreweryId { get; set; }
        [DisplayName("Brewery Name")]
        public string BreweryName { get; set; } 
    }
}