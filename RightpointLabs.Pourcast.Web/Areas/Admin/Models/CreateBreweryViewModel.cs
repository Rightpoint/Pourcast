using System.ComponentModel.DataAnnotations;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class CreateBreweryViewModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; } 
    }
}