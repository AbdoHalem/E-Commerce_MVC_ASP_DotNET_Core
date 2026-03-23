using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repos
{
    public interface IEntityRepo<T> where T : class
    {
        List<T> GetAll();  // Method to retrieve all records from the database
        T GetById(int id);  // Method to retrieve records by their ID
        void Add(T entity);  // Method to add a new record to the database
        void Update(T entity);  // Method to update an existing record's information in the database
        void Delete(int id);  // Method to delete a record from the database by its ID
        /// <summary>
        /// Method to find records based on a specified condition (using a lambda expression)
        /// We used includes params to enable eager looading to prevent (N+1) problem of lazy loading
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="includes"></param>
        /// <returns>A list of the T objects</returns>
        List<T> FindAll(Expression<Func<T, bool>> cond, params Expression<Func<T, object>>[] includes);  
    }
}
