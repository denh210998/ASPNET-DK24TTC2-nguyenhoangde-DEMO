using ecommerce_asp.Helpers;
using ecommerce_asp.Models;
using ecommerce_asp.Models.ViewModels;
using ecommerce_asp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View(model);
            }

            var user = new UserModel
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = PasswordHasher.Hash(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            TempData["success"] = "Register successfully!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !PasswordHasher.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);


            var cookieCartId = Request.Cookies["CartId"];

            if (cookieCartId != null)
            {

                var cookieCart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.SessionId == cookieCartId);

                if (cookieCart != null)
                {

                    var userCart = await _context.Carts
                        .Include(c => c.CartItems)
                        .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());


                    if (userCart == null)
                    {
                        cookieCart.UserId = user.Id.ToString();
                        cookieCart.SessionId = null;
                    }
                    else
                    {

                        foreach (var item in cookieCart.CartItems.ToList())
                        {
                            var exist = userCart.CartItems
                                .FirstOrDefault(c => c.ProductId == item.ProductId);

                            if (exist == null)
                            {

                                item.CartModelId = userCart.Id;
                            }
                            else
                            {
 
                                exist.Quanlity += item.Quanlity;

                                _context.CartItems.Remove(item);
                            }
                        }


                        _context.Carts.Remove(cookieCart);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["success"] = "Login successfully!";
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");

            TempData["success"] = "Logged out!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {

                return RedirectToAction("Login", "Account");
            }

            string userIdStr = userId.Value.ToString();

            var orders = await _context.Orders
                .Where(o => o.UserId == userIdStr)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userIdStr = userId.Value.ToString();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userIdStr);

            if (order == null)
            {
                return NotFound(); 
            }

            return View(order);  
        }
    }
}
