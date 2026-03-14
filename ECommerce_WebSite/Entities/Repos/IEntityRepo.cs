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
        public List<T> FindAll(Expression<Func<T, bool>> cond);  // Method to find records based on a specified condition (using a lambda expression)
    }
}
