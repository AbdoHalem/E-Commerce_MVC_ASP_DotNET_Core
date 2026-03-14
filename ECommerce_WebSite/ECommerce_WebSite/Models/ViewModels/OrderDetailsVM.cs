using Entities.Models;

namespace ECommerce_WebSite.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }
        public IEnumerable<Order_Item> Items { get; set; }
    }
}
