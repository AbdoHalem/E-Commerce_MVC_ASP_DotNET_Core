using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_WebSite.Models.ViewModels
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; } = new List<CartItemVM>();
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Please select a shipping address.")]
        public int SelectedAddressId { get; set; }
        // To display the user's addresses in a dropdown
        public IEnumerable<Address> UserAddresses { get; set; } = new List<Address>();
    }
}
