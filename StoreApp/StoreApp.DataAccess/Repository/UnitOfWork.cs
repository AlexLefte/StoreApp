using StoreApp.DataAccess.Data;
using StoreApp.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        ApplicationDbContext _db;
        #endregion

        public ICategoryRepository Category { get; private set; }

        #region Constructor
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
        }
        #endregion

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
