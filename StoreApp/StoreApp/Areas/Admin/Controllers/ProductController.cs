using Microsoft.AspNetCore.Mvc;
using StoreApp.DataAccess.Repository.IRepository;
using StoreApp.DataAccess.Repository;
using StoreApp.Models;
using StoreApp.Models.ViewModels;
using StoreApp.DataAccess.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructors
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            List<Product> productsList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productsList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().
                Select(cat => new SelectListItem
                {
                    Text = cat.Name,
                    Value = cat.Id.ToString()
                }),
                Product = new()
            };
            if (id != null && id != 0)
            {
                // Update
                productVM.Product = _unitOfWork.Product.Get(prod => prod.Id == id);
            }
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    // Adding the image to the images\product folder
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product", fileName);

                    string oldImage = productVM.Product.ImageUrl;
                    if (!string.IsNullOrEmpty(oldImage))
                    {
                        // Remove the old image:
                        oldImage = Path.Combine(wwwRootPath, oldImage.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStream = new FileStream(productPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                else
                {
                    productVM.Product.ImageUrl = @"\images\product\No_image.png";
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
 
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                // Return to the products list:
                return RedirectToAction("Index");
                // return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().
                    Select(cat => new SelectListItem
                    {
                        Text = cat.Name,
                        Value = cat.Id.ToString()
                    });
                return View(productVM);
            }      
        }

        /*public IActionResult Delete(int? id)
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

                // Return to the products list:
                return RedirectToAction("Index");
            }

            return View();
        }*/
        #endregion

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productsList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productsList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(product => product.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }

            string oldImage = productToBeDeleted.ImageUrl;
            if (!string.IsNullOrEmpty(oldImage))
            {
                // Remove the old image:
                oldImage = Path.Combine(_webHostEnvironment.WebRootPath, oldImage.TrimStart('\\'));

                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            List<Product> productsList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productToBeDeleted, success = true, message = "Product deleted successfully!" });
        }
        #endregion
    }
}
