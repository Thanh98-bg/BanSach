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
                TempData["Success"] = "Cover type create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CoverType cover = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (cover == null)
            {
                return NotFound();
            }
            return View(cover);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType cover)
        {
            if (cover == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(cover);
                _unitOfWork.Save();
                TempData["Success"] = "Edit cover type successfully";
                return RedirectToAction("index");
            }
            return View(cover);
        }
    }
}
