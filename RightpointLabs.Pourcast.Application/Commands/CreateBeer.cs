using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RightpointLabs.Pourcast.Application.Commands
{
    public class CreateBeer
    {
        public string BreweryId { get; set; }
        [DisplayName("Brewery Name")]
        public string BreweryName { get; set; }

        [Required(ErrorMessage = "Beer Name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        public double ABV { get; set; }
        [DisplayName("Beer Advocate Score")]
        public int BAScore { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Glass { get; set; }
    }
}