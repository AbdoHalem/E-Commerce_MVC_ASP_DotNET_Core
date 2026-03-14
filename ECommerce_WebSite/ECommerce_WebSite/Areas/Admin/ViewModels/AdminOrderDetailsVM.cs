using Entities.Models;

namespace ECommerce_WebSite.Areas.Admin.ViewModels
{
    public class AdminOrderDetailsVM
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        // Shipping address for this specific order
        public Address ShippingAddress { get; set; }

        // List of items purchased in this order
        public IEnumerable<Order_Item> Items { get; set; }
    }
}
