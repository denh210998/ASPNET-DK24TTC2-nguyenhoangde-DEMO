using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Models.CategoryModel> Categories { get; set; }
        public DbSet<Models.BrandModel> Brands { get; set; }
        public DbSet<Models.ProductModel> Products { get; set; }
    }
}
