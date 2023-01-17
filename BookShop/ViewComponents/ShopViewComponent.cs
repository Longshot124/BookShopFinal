using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
    public class ShopViewComponent : ViewComponent
    {
        private readonly BookDbContext _bookDbContext;

        public ShopViewComponent(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var books = await _bookDbContext.Books.Where(c => !c.IsDeleted).ToListAsync();
            return View(books);
        }
    }
}
