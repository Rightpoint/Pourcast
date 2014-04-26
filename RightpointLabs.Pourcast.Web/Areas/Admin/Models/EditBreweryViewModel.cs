using System.ComponentModel.DataAnnotations;
using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class EditBreweryViewModel
    {
        public string Id { get; set; }
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