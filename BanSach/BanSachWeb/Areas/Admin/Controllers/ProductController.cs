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
                productVM.product_ = _unitOfWork.Product.GetFirstOrDefault(u=> u.Id ==  id);
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
                    if (product.product_.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, product.product_.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, file_name + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    product.product_.ImageUrl = @"images\products\" + file_name + extension;
                }
                if (product.product_.Id == 0)
                {
                    _unitOfWork.Product.Add(product.product_);
                }
                else
                {
                    _unitOfWork.Product.Update(product.product_);
                }
                _unitOfWork.Save();
                TempData["Success"] = "product_ create successfully";
                return RedirectToAction("index");
            }
            return View(product);
        }

        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(icludeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                if (product.ImageUrl != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete successfully"});
            }
            return View(id);
        }
        #endregion
    }
}
