using Entities.Models;
using Entities.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UnitOfWork
{
    // IDisposable ensures the DbContext is properly closed and memory is freed
    public interface IUnitOfWork : IDisposable
    {
        // Properties for the Repos
        public IEntityRepo<App_User> userRepo { get; }
        public IEntityRepo<Address> addressRepo { get;  }
        public IEntityRepo<Order> orderRepo { get; }
        public IEntityRepo<Order_Item> orderItemRepo { get; }
        public IEntityRepo<Product> productRepo { get; }
        public IEntityRepo<Category> categoryRepo { get; }
        // Methods
        int SaveTransact();     // Save method to commit all changes across all repositories
    }
}
