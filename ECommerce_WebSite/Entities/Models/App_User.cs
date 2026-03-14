using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class App_User : IdentityUser
    {
        [Required]
        public String FullName { get; set; }
        // Navigation Properties
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
