using ecommerce_asp.Models;
using ecommerce_asp.Models.ViewModels;
using ecommerce_asp.Repository;
using ecommerce_asp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly DataContext _context;

        public CheckoutController(CartService cartService, DataContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // -------------------------
        // SHOW CHECKOUT PAGE
        // -------------------------
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetOrCreateCart();

            var items = cart.CartItems.Select(i => new CartItemModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Image = i.Image,
                Price = i.Price,
                Quanlity = i.Quanlity
            }).ToList();

            var vm = new CheckoutPageViewModel
            {
                Cart = new CartItemViewModel
                {
                    CartItems = items,
                    GrandTotal = items.Sum(x => x.Total)
                },
                Checkout = new CheckoutInputViewModel()
            };

            return View(vm);
        }

        // -------------------------
        // PLACE ORDER
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Please fill all required fields!";

                var cart = await _cartService.GetOrCreateCart();

                var items = cart.CartItems.Select(i => new CartItemModel
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Image = i.Image,
                    Price = i.Price,
                    Quanlity = i.Quanlity
                }).ToList();

                model.Cart = new CartItemViewModel
                {
                    CartItems = items,
                    GrandTotal = items.Sum(x => x.Total)
                };

                return View("Index", model);
            }

            // GET CART FROM DATABASE
            var userCart = await _cartService.GetOrCreateCart();

            // CLEAR CART AFTER ORDER
            _context.CartItems.RemoveRange(userCart.CartItems);
            await _context.SaveChangesAsync();

            TempData["success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
