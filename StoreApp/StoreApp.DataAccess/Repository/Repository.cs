using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreApp.DataAccess.Data;
using StoreApp.DataAccess.Repository.IRepository;

namespace StoreApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        #endregion

        #region Constructor
        public Repository(ApplicationDbContext db)
        {
            _db = db;       
            _dbSet = _db.Set<T>();
        }
        #endregion

        #region Methods
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        #endregion
    }
}
