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


        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return View(categories);
        }

    
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel model)
        {

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }

   
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

    
            model.Status = 1;

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Category created!";
            return RedirectToAction(nameof(Index));
        }

 
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel model)
        {
            if (id != model.Id) return NotFound();


            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Slug = model.Name.Trim().ToLower().Replace(" ", "-");
            }


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
