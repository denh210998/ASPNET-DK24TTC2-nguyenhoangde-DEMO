using ecommerce_asp.Models;
using ecommerce_asp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (model.ImageUpload == null || model.ImageUpload.Length == 0)
            {
                ModelState.AddModelError("ImageUpload", "Yêu cầu chọn hình ảnh");
            }

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            if (model.ImageUpload != null && model.ImageUpload.Length > 0)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid().ToString() +
                               Path.GetExtension(model.ImageUpload.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageUpload.CopyToAsync(fs);
                }

                model.Image = fileName;
            }

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            ModelState.Remove("ImageUpload");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            var oldProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (oldProduct == null) return NotFound();

            var uploadDir = Path.Combine(_env.WebRootPath, "images");

            if (model.ImageUpload != null && model.ImageUpload.Length > 0)
            {
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid().ToString() +
                               Path.GetExtension(model.ImageUpload.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageUpload.CopyToAsync(fs);
                }

                if (!string.IsNullOrEmpty(oldProduct.Image))
                {
                    var oldPath = Path.Combine(uploadDir, oldProduct.Image);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                model.Image = fileName;
            }
            else
            {
                model.Image = oldProduct.Image;
            }

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }

            _context.Products.Update(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();


            if (!string.IsNullOrEmpty(product.Image))
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");
                var filePath = Path.Combine(uploadDir, product.Image);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
