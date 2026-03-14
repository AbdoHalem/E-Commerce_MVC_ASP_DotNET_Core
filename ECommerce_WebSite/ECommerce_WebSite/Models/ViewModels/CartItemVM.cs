namespace ECommerce_WebSite.Models.ViewModels
{
    /**
     * It represents a product from the order-products
     */
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Expression-bodied property (C# 6.0) to dynamically calculate the total for this item
        public decimal LineTotal => Price * Quantity;   // Readonly Property
    }
}
