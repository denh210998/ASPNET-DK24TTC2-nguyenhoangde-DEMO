using ecommerce_asp.Models;
using ecommerce_asp.Models.ViewModels;
using ecommerce_asp.Repository;
using ecommerce_asp.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _context;
        private readonly CartService _cartService;

        public CartController(DataContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetOrCreateCart();

            var items = cart.CartItems.Select(i => new CartItemModel
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quanlity = i.Quanlity,
                Price = i.Price,
                Image = i.Image
            }).ToList();

            var vm = new CartItemViewModel
            {
                CartItems = items,
                GrandTotal = items.Sum(i => i.Total)
            };

            return View(vm);
        }

        public async Task<IActionResult> Add(int id)
        {
            var cart = await _cartService.GetOrCreateCart();
            var product = await _context.Products.FindAsync(id);

            if (cart.CartItems == null)
                cart.CartItems = new List<CartItemDBModel>();

            var item = cart.CartItems.FirstOrDefault(c => c.ProductId == id);

            if (item == null)
            {
                item = new CartItemDBModel
                {
                    CartModelId = cart.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quanlity = 1,
                    Price = product.Price,
                    Image = product.Image
                };

                _context.CartItems.Add(item);
            }
            else
            {
                item.Quanlity++;
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Add item to cart successfully!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Increase(int id)
        {
 
            var cart = await _cartService.GetOrCreateCart();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartModelId == cart.Id && c.ProductId == id);

            if (item == null)
                return RedirectToAction("Index"); 

            item.Quanlity++;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int id)
        {
            var cart = await _cartService.GetOrCreateCart();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartModelId == cart.Id && c.ProductId == id);

            if (item == null)
                return RedirectToAction("Index");

            if (item.Quanlity > 1)
            {
                item.Quanlity--;
            }
            else
            {
                _context.CartItems.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            var cart = await _cartService.GetOrCreateCart();

            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartModelId == cart.Id && c.ProductId == id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Clear()
        {
            var cart = await _cartService.GetOrCreateCart();
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Checkout()
        {
            return RedirectToAction("Index", "Checkout");
        }
    }
}
