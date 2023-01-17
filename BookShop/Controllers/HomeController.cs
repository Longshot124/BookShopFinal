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
            var slider = await _bookDbContext.Sliders.Include(e => e.Book).ThenInclude(e=>e.Author).Include(e => e.Book).ThenInclude(e=>e.Category).ToListAsync();
            var book = await _bookDbContext.Books.Where(e => !e.IsDeleted).ToListAsync();
            var homeViewModel = new HomeViewModel
            {
                Books = book,
                Sliders = slider,
            };
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}