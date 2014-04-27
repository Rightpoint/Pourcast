using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class ImportBeerViewModel
    {
        public string BreweryId { get; set; }
        [Display(Name = "Brewery")]
        public string BreweryName { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}