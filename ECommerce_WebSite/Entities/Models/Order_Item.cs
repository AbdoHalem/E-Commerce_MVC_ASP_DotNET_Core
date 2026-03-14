using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Order_Item
    {
        [Key]
        public int Id { get; set; }     // PK: Item Id
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }    // FK
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }  // FK
        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Decimal LineTotal { get; set; }
        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
