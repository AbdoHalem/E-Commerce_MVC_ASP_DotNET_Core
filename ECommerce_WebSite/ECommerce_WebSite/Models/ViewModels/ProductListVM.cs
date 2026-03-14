using Entities.Models;

namespace ECommerce_WebSite.Models.ViewModels
{
    public class ProductListVM
    {
        // List of products to display
        public IEnumerable<Product> Products { get; set; }
        // Filter state to remember what the user clicked
        public int? CurrentCategoryId { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentSort { get; set; }
        // Paging details
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        // List of categories for the filter dropdown or sidebar
        public IEnumerable<Category> Categories { get; set; }
    }
}
