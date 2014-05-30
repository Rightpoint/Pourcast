using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class CreateBeerViewModel
    {
        public CreateBeerViewModel() { }

        public CreateBeerViewModel(IEnumerable<Style> styles)
        {
            Styles = styles.Select(a => new SelectListItem() {Text = a.Name, Value = a.Id});
        }

        public string BreweryId { get; set; }
        [DisplayName("Brewery Name")]
        public string BreweryName { get; set; }
        [Required(ErrorMessage = "Beer Name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        public double ABV { get; set; }
        [DisplayName("Beer Advocate Score")]
        public double BAScore { get; set; }
        public IEnumerable<SelectListItem> Styles { get; set; }
        [DisplayName("Style")]
        [Required(ErrorMessage = "You must select a style for this beer.")]
        public string StyleId { get; set; }
 
    }
}