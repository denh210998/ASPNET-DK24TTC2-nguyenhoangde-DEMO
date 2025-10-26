using ecommerce_asp.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel iphone= new CategoryModel { Name = "iphone", Description="Apple Products", Slug = "iphone", Status = 1};
                CategoryModel airpod= new CategoryModel { Name = "airpod", Description="Apple Products", Slug = "airpod", Status = 1};
                BrandModel apple = new BrandModel { Name = "apple", Description = "Apple Products", Slug = "apple", Status = 1 };

                _context.Products.AddRange(
                    new ProductModel { Name = "iPhone 13 Pro Max", Description = "The iPhone 13 Pro Max is a smartphone designed and marketed by Apple Inc. It is part of the iPhone 13 series and was announced on September 14, 2021. The iPhone 13 Pro Max features a larger display, improved camera system, and enhanced performance compared to its predecessor, the iPhone 12 Pro Max.", Slug = "iphone", Price = 1099.00M, Category = iphone, Brand = apple, Image = "iphone13promax.jpg" },
                    new ProductModel { Name = "iPhone 12", Description = "The iPhone 12 is a smartphone designed and marketed by Apple Inc. It is part of the iPhone 12 series and was announced on October 13, 2020. The iPhone 12 features a new design with flat edges, a Super Retina XDR display, and support for 5G connectivity.", Slug = "iphone", Price = 799.00M, Category = iphone, Brand = apple, Image = "iphone12.jpg" },
                    new ProductModel { Name = "AirPods Pro", Description = "AirPods Pro are wireless earbuds designed and marketed by Apple Inc. They were announced on October 30, 2019, and released on October 30, 2019. AirPods Pro feature active noise cancellation, a customizable fit with silicone ear tips, and improved sound quality compared to the standard AirPods.", Slug = "airpod", Price = 249.00M, Category = airpod, Brand = apple, Image = "airpodspro.jpg" },
                    new ProductModel { Name = "AirPods (2nd generation)", Description = "The AirPods (2nd generation) are wireless earbuds designed and marketed by Apple Inc. They were announced on March 20, 2019, and released on March 26, 2019. The AirPods (2nd generation) feature the H1 chip for improved connectivity and hands-free 'Hey Siri' functionality.", Slug = "airpod", Price = 159.00M, Category = airpod, Brand = apple, Image = "airpods2ndgen.jpg" }
                );
                _context.SaveChanges();
            }
        }
    }
}
