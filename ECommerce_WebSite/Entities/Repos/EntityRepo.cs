using Entities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repos
{
    public class EntityRepo<T> : IEntityRepo<T> where T : class
    {
        ECommContext _context;
        DbSet<T> _set;

        public EntityRepo(ECommContext context)
        {
            _context = context;
            _set = _context.Set<T>(); // Get the DbSet of the used class (e.g. App_User, Order)
        }

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void Delete(int id)
        {
            _set.Remove(GetById(id));
        }

        public List<T> FindAll(Expression<Func<T, bool>> cond, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.Where(cond);
            // Add includes to the query to enable eager loading
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.ToList();
        }

        public List<T> GetAll()
        {
            return _set.ToList();
        }

        public T GetById(int id)
        {
            return _set.Find(id)!;
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }
    }
}
