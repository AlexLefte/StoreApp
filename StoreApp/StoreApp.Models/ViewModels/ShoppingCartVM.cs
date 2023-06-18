using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCarts { get; set; }
        public OrderHeader OrderHeader { get; set; }

        /*public ShoppingCartVM(IEnumerable<ShoppingCart> shoppingCarts)
        {
            ShoppingCarts = shoppingCarts;
            OrderHeader = new()
            {
                TotalOrder = ComputeTotalOrder()
            };
        }*/

        public double ComputeTotalOrder()
        {
            double total = 0;
            foreach (ShoppingCart shoppingCart in ShoppingCarts ?? Enumerable.Empty<ShoppingCart>())
            {
                shoppingCart.Price = GetPriceBaseOnQuantity(shoppingCart);
                total += shoppingCart.Price * shoppingCart.Count;
            }
            return total;
        }

        private double GetPriceBaseOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}
