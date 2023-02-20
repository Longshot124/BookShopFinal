using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public BlogController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var blog = await _bookDbContext.Blogs.Where(e => !e.IsDeleted).ToListAsync();


            int perPage = 1;
            int pageCount = (int)Math.Ceiling((double)blog.Count() / perPage);
            if (page <= 0) page = 1;
            if (page > pageCount) page = pageCount;

            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;

            return View(blog.Skip((page - 1) * perPage).Take(perPage).ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var blog = await _bookDbContext.Blogs.Where(c => !c.IsDeleted && c.Id == id)
                .Include(a => a.BlogCategory)
                
                .FirstOrDefaultAsync();
            if (blog is null) return NotFound();

            return View(blog);
        }

        public async Task<IActionResult> BlogSidebarCategory(int? id)
        {
            var blogCategories = await _bookDbContext.BlogCategories
                .Where(c => c.Id == id)
                .Include(c => c.Blogs)
                .ToListAsync();
            var blogs = await _bookDbContext.Blogs
                .Where(b => !b.IsDeleted)
                .ToListAsync();
            var blogViewModel = new BlogCategorySidebarViewModel
            {
                BlogCategories = blogCategories,
                Blogs = blogs
            };
            return View(blogViewModel);

        }
    }
}
