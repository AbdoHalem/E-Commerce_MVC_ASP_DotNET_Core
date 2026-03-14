using System.ComponentModel.DataAnnotations;

namespace ECommerce_WebSite.Models.ViewModels
{
    public class AddressVM
    {
        [Required(ErrorMessage = "Country is required")]
        [StringLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Street name/number is required")]
        [StringLength(100)]
        public string Street { get; set; }

        [Required(ErrorMessage = "ZIP Code is required")]
        [StringLength(20)]
        [Display(Name = "ZIP Code")]
        public string Zip { get; set; }

        [Display(Name = "Set as default address")]
        public bool IsDefault { get; set; }
    }
}
