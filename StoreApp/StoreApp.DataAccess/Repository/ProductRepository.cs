using StoreApp.DataAccess.Data;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        #region Fields
        ApplicationDbContext _db;
        #endregion

        #region Constructor
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
        #endregion
    }
}
