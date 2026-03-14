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
    public class Product
    {
        [Key]
        public int Id { get; set; }     // PK; Product Id
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; } // FK
        public required String Name { get; set; }    // Product name
        public String SKU { get; set; }     // UK
        public Decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        // Add New property for the image path
        public string ImageUrl { get; set; } = "/images/products/default.png";
        // Add the Description property with a default empty string to avoid null issues
        public string Description { get; set; } = string.Empty;
        // Navigation Properties
        public virtual ICollection<Order_Item> Items { get; set; } = new HashSet<Order_Item>();
        public virtual Category Category { get; set; }
    }
}
