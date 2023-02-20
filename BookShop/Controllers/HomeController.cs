using BookShop.BLL.Services;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public HomeController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {

            var slider = await _bookDbContext.Sliders
                .Where(e => !e.IsDeleted)
                .Include(e => e.Book).ThenInclude(e => e.Author)
                .Include(e => e.Book).ThenInclude(e => e.Category)
                .ToListAsync();
            var book = await _bookDbContext.Books.Where(e => !e.IsDeleted).ToListAsync();
            var category = await _bookDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var homeViewModel = new HomeViewModel
            {
                Books = book,
                Sliders = slider,
                Categories = category,

            };

            return View(homeViewModel);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return NoContent();

            var books = await _bookDbContext.Books
                .Where(book => !book.IsDeleted && book.Name.ToLower().StartsWith(searchText))
                .ToListAsync();

            var model = new List<Book>();

            books.ForEach(book => model.Add(new Book
            {
                Id = book.Id,
                Name = book.Name,
                ImageUrl = book.ImageUrl,
            }));

            return PartialView("_BookSearchPartial", books);
        }

        public IActionResult Error404(int code)
        {
            ErrorViewModel error = new ErrorViewModel()
            {
                StatusCode = HttpContext.Response.StatusCode,
                Title = HttpContext.Response.Headers.ToString()

            };
            return View();
        }

    }
}