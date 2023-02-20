using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public ShopController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var books = await _bookDbContext.Books
                .Where(c => !c.IsDeleted)
                .Include(c => c.Category)
                .Include(c => c.Author)
                .Include(c => c.Publisher)
                .ToListAsync();

            int perPage = 4;
            int pageCount = (int)Math.Ceiling((double)books.Count() / perPage);
            if (page <= 0) page = 1;
            if (page > pageCount) page = pageCount;

            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;

            return View(books.Skip((page - 1) * perPage).Take(perPage).ToList());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var book = await _bookDbContext.Books.Where(c => !c.IsDeleted && c.Id == id)
                .Include(a=>a.Author)
                .Include(a => a.Category)
                .Include(a => a.Publisher)
                .Include(a => a.BookLanguage)
                .FirstOrDefaultAsync();
            if (book is null) return NotFound();

            return View(book);
        }

        

        public async Task<IActionResult> ShopSideBarCategory(int? id, int page = 1)
        {
            var categories = await _bookDbContext.Categories
                .Where(c => c.Id == id)
                .Include(c => c.Books).ThenInclude(a=>a.Author)
                .ToListAsync();

            int perPage = 8;
            int pageCount = (int)Math.Ceiling((double)categories.Count() / perPage);

            if (page <= 0) page = 1;
            if (page > pageCount) page = pageCount;

            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;
            return View(categories.Skip((page - 1) * perPage).Take(perPage).ToList());
        }

    }
}
