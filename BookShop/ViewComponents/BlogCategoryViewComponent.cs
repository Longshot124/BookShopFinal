using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
    public class BlogCategoryViewComponent : ViewComponent
    {
        private readonly BookDbContext _bookDbContext;

        public BlogCategoryViewComponent(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogCategories = await _bookDbContext.BlogCategories.Where(c => !c.IsDeleted).Include(b=>b.Blogs).ToListAsync();
            var blogs = await _bookDbContext.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            var model = new BlogCategorySidebarViewModel
            {
                BlogCategories = blogCategories,
                Blogs = blogs,
            };
            return View(model);
        }
    }
}
