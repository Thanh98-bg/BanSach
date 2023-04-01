using BanSach.DataAccess.Repository;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product create successfully";
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
            Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["Success"] = "Edit cover type successfully";
                return RedirectToAction("index");
            }
            return View(product);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                TempData["Success"] = "Remove product successfully";
                return RedirectToAction("index");
            }
            return View(id);
        }
    }
}
