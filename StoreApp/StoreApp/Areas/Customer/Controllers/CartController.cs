using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.Models;
using StoreApp.Models.ViewModels;
using StoreApp.Utility;
using Stripe.Checkout;
using System.Security.Claims;

namespace StoreApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        [BindProperty]
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

            // Creating the ShoppingCartVM
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCarts = shoppingCarts,
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.TotalOrder = ShoppingCartVM.ComputeTotalOrder();
            // ShoppingCartVM = new ShoppingCartVM(shoppingCarts);

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(shoppingCart => shoppingCart.UserId == userId,
                includeProperties: "Product");

            // Creating the ShoppingCartVM
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCarts = shoppingCarts,
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.TotalOrder = ShoppingCartVM.ComputeTotalOrder();
            ShoppingCartVM.OrderHeader.User = _unitOfWork.User.Get(user => user.Id == userId);
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(shoppingCart => shoppingCart.UserId == userId,
                includeProperties: "Product");
            ShoppingCartVM.ShoppingCarts = shoppingCarts;
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.UserId = userId;    
            // ShoppingCartVM.OrderHeaders.User = _unitOfWork.User.Get(user => user.Id == userId);
            User user = _unitOfWork.User.Get(user => user.Id == userId);
            ShoppingCartVM.OrderHeader.TotalOrder = ShoppingCartVM.ComputeTotalOrder();

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach(var cart in ShoppingCartVM.ShoppingCarts)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                // Stripe logic for regular customers
                var domain = "https://localhost:7291/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach(var item in ShoppingCartVM.ShoppingCarts)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(order => order.Id == id, includeProperties: "User");
            if  (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                // Order by customer:
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(cart => cart.UserId == orderHeader.UserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
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
