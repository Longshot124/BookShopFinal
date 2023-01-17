using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
	public class CategoryController : Controller
	{
        private readonly BookDbContext _bookDbContext;

        public CategoryController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }


        public async Task<IActionResult> Index()
		{
            var categories = await _bookDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();

            var viewModel = new HomeViewModel
            {
                Categories = categories
            };

            return View(viewModel);
        }
	}
}
