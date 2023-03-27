using BanSach.DataAccess.Repository;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> covers = _unitOfWork.CoverType.GetAll();
            return View(covers);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
    }
}
