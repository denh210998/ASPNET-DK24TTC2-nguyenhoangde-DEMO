using ecommerce_asp.Models;
using ecommerce_asp.Repository;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Services
{
    public class CartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _http;

        public CartService(DataContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        private string GetOrCreateCookieCartId()
        {
            var ctx = _http.HttpContext;

            if (ctx.Request.Cookies.ContainsKey("CartId"))
                return ctx.Request.Cookies["CartId"];

            var newId = Guid.NewGuid().ToString();

            ctx.Response.Cookies.Append("CartId", newId, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            });

            return newId;
        }

        public async Task<CartModel> GetOrCreateCart()
        {
            var ctx = _http.HttpContext;
            var userId = ctx.Session.GetInt32("UserId");

            CartModel cart = null;

            if (userId != null)
            {
                cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId.ToString());

                if (cart != null)
                    return cart;
            }

            string cookieId = GetOrCreateCookieCartId();

            cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.SessionId == cookieId);

            if (cart == null)
            {
                cart = new CartModel
                {
                    SessionId = cookieId,
                    CartItems = new List<CartItemDBModel>()
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
