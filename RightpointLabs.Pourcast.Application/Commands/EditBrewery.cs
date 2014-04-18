using System.ComponentModel.DataAnnotations;

namespace RightpointLabs.Pourcast.Application.Commands
{
    public class EditBrewery
    {
        public string Id { get; set; }
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
