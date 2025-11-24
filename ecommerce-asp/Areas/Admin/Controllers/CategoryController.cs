using ecommerce_asp.Models;
using ecommerce_asp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        // ========== INDEX ==========
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return View(categories);
        }

        // ========== CREATE GET ==========
        [HttpGet]
        public IActionResult Create() => View();

        // ========== CREATE POST ==========
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            // Tạo slug từ Name
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }

            // Check slug trùng
            bool slugExists = await _context.Categories
                .AnyAsync(c => c.Slug == model.Slug);
            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Status default
            model.Status = 1;

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category created!";
            return RedirectToAction(nameof(Index));
        }

        // ========== EDIT GET ==========
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // ========== EDIT POST ==========
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel model)
        {
            if (id != model.Id) return NotFound();

            // Tạo lại slug từ Name
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }

            // Check slug trùng, loại trừ chính nó
            bool slugExists = await _context.Categories
                .AnyAsync(c => c.Id != model.Id && c.Slug == model.Slug);
            if (slugExists)
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.Categories.Update(model);
                await _context.SaveChangesAsync();

                TempData["success"] = "Category updated!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(c => c.Id == model.Id))
                    return NotFound();

                throw;
            }
        }

        // ========== DELETE ==========
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
