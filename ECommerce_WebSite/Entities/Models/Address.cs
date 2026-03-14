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
    public class Address
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        [ForeignKey(nameof(User)), Required]
        public String UserId { get; set; }  // Foreign Key of table App_User
        public String Country { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String Zip { get; set; }
        public bool IsDefault { get; set; }
        // Navigation Properties
        public virtual App_User User { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
