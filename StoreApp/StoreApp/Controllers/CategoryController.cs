using Microsoft.AspNetCore.Mvc;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }


        #region Methods
        public IActionResult Index()
        {
            List<Category> categoriesList = _db.Categories.ToList();
            return View(categoriesList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order must not match the Name.");
            }

            /*if (category.Name == "test")
            {
                ModelState.AddModelError("", "Test is an invalid value");
            }*/

            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully!";
                // Return to the categories list:
                return RedirectToAction("Index");
                // return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.FirstOrDefault(cat => cat.Id == id);
            
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();

                TempData["success"] = "Category updated successfully!";

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
            Category? categoryFromDb = _db.Categories.FirstOrDefault(cat => cat.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.FirstOrDefault(cat => cat.Id == id);
            if (obj == null)
            {
                return NotFound();
            }    

            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                _db.Categories.Remove(obj);
                _db.SaveChanges();

                TempData["success"] = "Category deleted successfully!";

                // Return to the categories list:
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion
    }
}
