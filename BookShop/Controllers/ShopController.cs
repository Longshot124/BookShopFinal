using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var books = await _bookDbContext.Books
                .Where(c => !c.IsDeleted)
                .Include(c => c.Category)
                .Include(c => c.Author)
                .Include(c => c.Publisher)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                Books = books
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var book = await _bookDbContext.Books.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (book is null) return NotFound();

            return View(book);
        }
        //public async Task<IActionResult> Search(string searchText)
        //{
        //    if (string.IsNullOrEmpty(searchText))
        //        return NoContent();

        //    var books = await _bookDbContext.Books
        //        .Where(book => !book.IsDeleted && book.Name.ToLower().Contains(searchText.ToLower()))
        //        .ToListAsync();

        //    var model = new List<Book>();

        //    books.ForEach(book => model.Add(new Book
        //    {
        //        Id = book.Id,
        //        ImageUrl = book.ImageUrl,
        //        BookInfo= book.BookInfo,
        //    }));

        //    return View(model);

        //    //return PartialView("_CourseSearchPartial", courses);
        //}

        public async Task<IActionResult> BlogSidebar(int? id)
        {
            var categories = await _bookDbContext.Categories.Where(c => c.Id == id).Include(c => c.Books).ToListAsync();
            return View(categories);
        }

    }
}
