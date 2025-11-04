using ecommerce_asp.Models;
using ecommerce_asp.Models.ViewModels;
using ecommerce_asp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_asp.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;

        public CartController(DataContext _context)
        {
            _dataContext = _context;
        }

        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quanlity * x.Price)
            };
            return View(cartVM);
        }

        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
        public async Task<IActionResult> Add(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems= cart.Where(c=>c.ProductId == Id).FirstOrDefault();
            if (cartItems == null) 
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quanlity += 1;
            }
            HttpContext.Session.SetJson("Cart" ,cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
