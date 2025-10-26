using ecommerce_asp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_asp.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext context)
        {
            _dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int Id)
        {
            if(Id == null) return RedirectToAction("Index", "Home");
            var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            return View(productsById);
        }
    }
}
