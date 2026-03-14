using System.ComponentModel.DataAnnotations;

namespace ECommerce_WebSite.Areas.Admin.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50)]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required")]
        [Range(0, 10000, ErrorMessage = "Stock cannot be negative")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // This holds the string path to display the existing image in the Edit view
        public string? ImageUrl { get; set; }

        // This receives the actual uploaded file from the HTML input type="file"
        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
