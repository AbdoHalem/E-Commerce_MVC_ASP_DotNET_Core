using Entities.Data;
using Entities.Models;
using Entities.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        ECommContext _context;
        // Private fields for repositories
        IEntityRepo<App_User> _userRepo;
        IEntityRepo<Address> _addressRepo;
        IEntityRepo<Order> _orderRepo;
        IEntityRepo<Order_Item> _orderItemRepo;
        IEntityRepo<Product> _productRepo;
        IEntityRepo<Category> _categoryRepo;
        // Inject the DbContext once
        public UnitOfWork(ECommContext context)
        {
            _context = context;
        }
        // Lazy initialization for Repositories (Only create them if requested)
        public IEntityRepo<App_User> userRepo => _userRepo ??= new EntityRepo<App_User>(_context);
        public IEntityRepo<Address> addressRepo => _addressRepo ??= new EntityRepo<Address>(_context);
        public IEntityRepo<Order> orderRepo => _orderRepo ??= new EntityRepo<Order>(_context);
        public IEntityRepo<Order_Item> orderItemRepo => _orderItemRepo ??= new EntityRepo<Order_Item>(_context);
        public IEntityRepo<Product> productRepo => _productRepo ??= new EntityRepo<Product>(_context);
        public IEntityRepo<Category> categoryRepo => _categoryRepo ??= new EntityRepo<Category>(_context);
        /**
         * Saves all changes made in the context to the database
         */
        public int SaveTransact()
        {
            return _context.SaveChanges();
        }
        /**
         * Clean up the context when done
         */
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
