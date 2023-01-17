using BookShop.Data.DAL;
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
            var categories = await _bookDbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View(categories);
        }
    }
}
