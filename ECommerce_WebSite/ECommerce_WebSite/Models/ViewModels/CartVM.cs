namespace ECommerce_WebSite.Models.ViewModels
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();

        // Readonly property to dynamically calculate the total amount of the whole cart
        public decimal TotalAmount => Items.Sum(i => i.LineTotal);
    }
}
