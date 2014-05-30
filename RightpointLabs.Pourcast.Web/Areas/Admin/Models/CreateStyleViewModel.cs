using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RightpointLabs.Pourcast.Web.Areas.Admin.Models
{
    public class CreateStyleViewModel
    {
        [DisplayName("Style Name")]
        [Required(ErrorMessage = "Style Name is required")]
        public string Name { get; set; }
        [DisplayName("Color (As hex value)")]
        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }
        [Required]
        public string Glass { get; set; }
    }
}