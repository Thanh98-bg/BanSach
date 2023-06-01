using BanSach.DataAccess.Repository.IRepository;
using BanSach.Model;
using BanSach.Model.ViewModel;
using BanSach.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BanSachWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
	[BindProperties]
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
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "product"),
                OrderHeader = new()
            };
            foreach(var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.product.Price50, cart.product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Count * cart.Price;
			}
			return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "product"),
				OrderHeader = new()
			};
			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
			if (ShoppingCartVM.OrderHeader.ApplicationUser != null)
			{
				ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
				ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
				ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
				ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
				ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
				ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
			}
			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.product.Price50, cart.product.Price100);
				ShoppingCartVM.OrderHeader.OrderTotal += cart.Count * cart.Price;
			}
			return View(ShoppingCartVM);
		}
		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPost()
		{
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "product");
			ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.product.Price50, cart.product.Price100);
				ShoppingCartVM.OrderHeader.OrderTotal += cart.Count * cart.Price;
			}
			_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitOfWork.Save();
			foreach (var cart in ShoppingCartVM.ListCart)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = cart.ProductId,
					OrderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count
				};
				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}
			_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
			_unitOfWork.Save();
			return RedirectToAction("Index","Home");
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
