using BanSach.DataAccess.Repository;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BanSach.Model.ViewModel;

namespace BanSachWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM();
            productVM.product_ = new Product();
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            if (id == null || id == 0)
            {
                //create product_
                return View(productVM);
            }
            else
            {
                //update product_
            }
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM product, IFormFile? file)
        {
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //upload images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string file_name = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, file_name + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    product.product_.ImageUrl = @"images\products" + file_name + extension;
                }
                _unitOfWork.Product.Add(product.product_);
                _unitOfWork.Save();
                TempData["Success"] = "product_ create successfully";
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
                TempData["Success"] = "Remove product_ successfully";
                return RedirectToAction("index");
            }
            return View(id);
        }
    }
}
