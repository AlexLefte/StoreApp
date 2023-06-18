using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.Models;
using StoreApp.Models.ViewModels;
using System.Security.Claims;

namespace StoreApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(shoppingCart => shoppingCart.UserId == userId,
                includeProperties: "Product");
            ShoppingCartVM = new ShoppingCartVM(shoppingCarts);

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(cart => cart.Id == cartId);
            shoppingCart.Count++;
            _unitOfWork.ShoppingCart.Update(shoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(cart => cart.Id == cartId);
            if (shoppingCart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
            }
            else
            {
                shoppingCart.Count--;
                _unitOfWork.ShoppingCart.Update(shoppingCart);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(cart => cart.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(shoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
