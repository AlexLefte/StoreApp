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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        #region Fields
        ApplicationDbContext _db;
        #endregion

        #region Constructor
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Methods
        
        public void Update(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
        }
        #endregion
    }
}
