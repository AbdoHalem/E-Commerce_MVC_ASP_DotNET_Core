namespace ECommerce_WebSite.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
