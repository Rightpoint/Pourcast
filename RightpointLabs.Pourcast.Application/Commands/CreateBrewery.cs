using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RightpointLabs.Pourcast.Application.Commands
{
    public class CreateBrewery
    {
        [Required(ErrorMessage = "Brewery Name is required.")]
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
    }
}