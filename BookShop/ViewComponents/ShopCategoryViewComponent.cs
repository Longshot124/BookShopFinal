using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
    public class ShopCategoryViewComponent : ViewComponent
    {
        private readonly BookDbContext _bookDbContext;

        public ShopCategoryViewComponent(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _bookDbContext.Categories.Where(c => !c.IsDeleted).Include(b => b.Books).ToListAsync();
            var model = new ShopCategoryViewModel
            {
                Categories = categories,
            };

            return View(model);
        }
    }
}
