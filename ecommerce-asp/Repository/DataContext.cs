using ecommerce_asp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace ecommerce_asp.Repository
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Models.CategoryModel> Categories { get; set; }
        public DbSet<Models.BrandModel> Brands { get; set; }
        public DbSet<Models.ProductModel> Products { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemDBModel> CartItems { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
