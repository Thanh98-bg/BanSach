using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;
        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.GetAll();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == null)
            {
                ModelState.AddModelError("CustomError","The name must not be empty");
                //ModelState.AddModelError("name","The name must not be empty");
            }
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.Save();
                TempData["Success"] = "Category create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.GetFirstOrDefault(x=>x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == null || obj.Name == "")
            {
                ModelState.AddModelError("CustomError", "The name must not be empty");
            }
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.Save();
                TempData["Success"] = "Category update successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Category? remove = _db.GetFirstOrDefault(y => y.Id == id);
            if (remove != null && ModelState.IsValid)
            {
                _db.Remove(remove);
                _db.Save();
                TempData["Success"] = "Category delete successfully";
                return RedirectToAction("index");
            }
            return View();
        }
    }
}
