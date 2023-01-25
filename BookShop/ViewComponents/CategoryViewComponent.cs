using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
	public class CategoryViewComponent:ViewComponent
	{
        private readonly BookDbContext _bookDbContext;

        public CategoryViewComponent(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _bookDbContext.Categories.Where(c => !c.IsDeleted).Include(b => b.Books).ToListAsync();
            var books = await _bookDbContext.Books.Where(b => !b.IsDeleted).ToListAsync();
            var model = new CategoryViewModel
            {
                Categories = categories,
                Books = books,
            };

            return View(model);
        }
    }
}
