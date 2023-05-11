using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> companies = _unitOfWork.Company.GetAll();
            return View(companies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company) 
        {
            if (company == null)
            {
                ModelState.AddModelError("CustomError", "Input error!");
            }
            else if (ModelState.IsValid)
            {
                _unitOfWork.Company.Add(company);
                _unitOfWork.Save();
                TempData["Success"] = "Company create successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Company company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (company == null)
            {
                ModelState.AddModelError("CustomError", "Invalid company");
            } 
            else if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(company);
                _unitOfWork.Save();
                TempData["Success"] = "Company update successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Company company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if (company != null && ModelState.IsValid)
            {
                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();
                TempData["Success"] = "Delete company successfully";
                return RedirectToAction("Index");
            }
            return View(id);
        }
    }
}
