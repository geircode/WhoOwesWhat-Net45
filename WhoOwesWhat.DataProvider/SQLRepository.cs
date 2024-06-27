using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WhoOwesWhat.DataProvider
{
    public interface ISqlRepository<T>
    {
        T Add(T item);
        void Remove(T item);
        IQueryable<T> GetAll();

        /// <summary>
        /// Executes Query
        /// </summary>
        /// <returns></returns>
        List<T> GetAllAsList();
        //T GetSingleOrDefault(Expression<Func<T, bool>> expression);
        void SaveChanges();
        //T GetFirstOrDefault(Func<T, bool> func);
    }

    public class SqlRepository<T> : ISqlRepository<T>
        where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public SqlRepository(DbContext context, DbSet<T> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public T Add(T item)
        {
            _dbSet.Add(item);
            return item;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public List<T> GetAllAsList()
        {
            return _dbSet.ToList();
        }

        public T GetSingleOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbSet.SingleOrDefault(expression);
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public T GetFirstOrDefault(Func<T, bool> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }
    }
}
