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
            var prodFromDb = _db.Products.FirstOrDefault(prod => prod.Id == product.Id);
            if (prodFromDb != null)
            {
                prodFromDb.Title = product.Title;
                prodFromDb.ISBN = product.ISBN;
                prodFromDb.Price = product.Price;
                prodFromDb.ListPrice = product.ListPrice;
                prodFromDb.Description = product.Description;
                prodFromDb.Category = product.Category;
                prodFromDb.Price50 = product.Price50;
                prodFromDb.Price100 = product.Price100;
                prodFromDb.Author = product.Author;
                prodFromDb.CategoryId = product.CategoryId;

                // Update image URL only if it is not null
                if (product.ImageUrl != null)
                {
                    prodFromDb.ImageUrl = product.ImageUrl;
                }
            }
            if (prodFromDb != null)
            {
                _db.Products.Update(prodFromDb);
            }          
        }
        #endregion
    }
}
