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
    public class CompanyController : Controller
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructors
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            List<Company> CompanysList = _unitOfWork.Company.GetAll().ToList();
            return View(CompanysList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id != null && id != 0)
            {
                // Update
                return View(_unitOfWork.Company.Get(comp => comp.Id == id));
            }
            return View(new Company());
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            // Check whether the model is valid
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully!";
                // Return to the Companys list:
                return RedirectToAction("Index");
                // return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(new Company());
            }
        }
        #endregion

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanysList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = CompanysList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(Company => Company.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            List<Company> CompanysList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = CompanyToBeDeleted, success = true, message = "Company deleted successfully!" });
        }
        #endregion
    }
}
