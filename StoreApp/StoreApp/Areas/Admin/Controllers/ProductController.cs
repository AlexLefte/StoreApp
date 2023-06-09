using Microsoft.AspNetCore.Mvc;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.DataAccess.Repository;
using StoreApp.Models;
using StoreApp.DataAccess.Data;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            List<Product> categoriesList = _unitOfWork.Product.GetAll().ToList();
            return View(categoriesList);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product Product)
        {
            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                // Return to the categories list:
                return RedirectToAction("Index");
                // return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProductFromDb = _unitOfWork.Product.Get(cat => cat.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product Product)
        {
            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(Product);
                _unitOfWork.Save();

                TempData["success"] = "Product updated successfully!";

                // Return to the categories list:
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProductFromDb = _unitOfWork.Product.Get(cat => cat.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(cat => cat.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Remove(obj);
                _unitOfWork.Save();

                TempData["success"] = "Product deleted successfully!";

                // Return to the categories list:
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion
    }
}
