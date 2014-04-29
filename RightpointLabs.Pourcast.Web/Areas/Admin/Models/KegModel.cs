using System.ComponentModel.DataAnnotations;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class KegModel
    {
        public string Id { get; set; }
        [Display(Name = "Beer")]
        public string BeerName { get; set; }
        [Display(Name = "Amount of Beer Remaining")]
        public double AmountOfBeerRemaining { get; set; }
        [Display(Name = "Percent Remaining")]
        public double PercentRemaining { get; set; }
        [Display(Name = "Empty")]
        public bool IsEmpty { get; set; }
        [Display(Name = "On Tap")]
        public bool IsOnTap { get; set; }
    }
}