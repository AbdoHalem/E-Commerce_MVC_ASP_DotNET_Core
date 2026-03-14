using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User)), Required]
        public String UserId { get; set; }
        [ForeignKey(nameof(Address)), Required]
        public int ShippingAddressId { get; set; }
        public String OrderNumber { get; set; } // UK: Unique
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalAmount { get; set; }
        // Navigation Properties
        public virtual ICollection<Order_Item> Items { get; set; } = new HashSet<Order_Item>();
        public virtual Address Address { get; set; }
        public virtual App_User User { get; set; }
    }
}
