using StoreApp.DataAccess.Data;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        #region Fields
        ApplicationDbContext _db;
        #endregion

        #region Constructor
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods
        
        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
        #endregion
    }
}
