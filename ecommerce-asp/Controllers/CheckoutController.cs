
using ecommerce_asp.Models;
using ecommerce_asp.Models.ViewModels;
using ecommerce_asp.Repository;
using ecommerce_asp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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

        public async Task<IActionResult> Index()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {

                return RedirectToAction("Login", "Account");
            }

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

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutPageViewModel model)
        {
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

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

            var userCart = await _cartService.GetOrCreateCart();

            if (userCart.CartItems == null || !userCart.CartItems.Any())
            {
                TempData["error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            var order = new OrderModel
            {
                FullName = model.Checkout.FullName,
                Email = model.Checkout.Email,
                Phone = model.Checkout.Phone,
                Address = model.Checkout.Address,
                Note = model.Checkout.Note,
                TotalAmount = userCart.CartItems.Sum(i => i.Quanlity * i.Price),
                CreatedAt = DateTime.Now,

                UserId = sessionUserId.Value.ToString()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = userCart.CartItems.Select(i => new OrderItemModel
            {
                OrderId = order.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Image = i.Image,
                Quantity = i.Quanlity,
                Price = i.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);

            _context.CartItems.RemoveRange(userCart.CartItems);
            await _context.SaveChangesAsync();

            TempData["success"] = "Order placed successfully!";
            return RedirectToAction("OrderSuccess", new { id = order.Id });
        }

        public async Task<IActionResult> OrderSuccess(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }
}
