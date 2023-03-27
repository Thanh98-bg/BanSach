using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<CoverType> covers = new List<CoverType>();
            return View(covers);
        }
    }
}
