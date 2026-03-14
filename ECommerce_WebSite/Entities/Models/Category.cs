using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }    // UK
        [AllowNull]
        public int? ParentCategoryId { get; set; }
        // Navigation Properties
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
