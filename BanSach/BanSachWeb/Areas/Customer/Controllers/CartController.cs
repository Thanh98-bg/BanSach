using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using BanSach.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BanSachWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "product")
            };
            foreach(var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.product.Price50, cart.product.Price100);
				ShoppingCartVM.CartTotal += cart.Count * cart.Price;
			}
			return View(ShoppingCartVM);
        }
        private double GetPriceBaseOnQuantity(int quantity, double price50, double price100)
        {
            if (quantity < 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }
        public IActionResult Plus(int cartId)
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart != null)
            {
				_unitOfWork.ShoppingCart.IncrementCount(cart, 1);
				_unitOfWork.Save();
			}
            return RedirectToAction(nameof(Index));
        }
		public IActionResult Minus(int cartId)
		{
			ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
			if (cart.Count > 1)
			{
				_unitOfWork.ShoppingCart.DecrementCount(cart, 1);
			} else
            {
				_unitOfWork.ShoppingCart.Remove(cart);
			}
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Remove(int cartId)
		{
			ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
			if (cart != null)
			{
				_unitOfWork.ShoppingCart.Remove(cart);
				_unitOfWork.Save();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
