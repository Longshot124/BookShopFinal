using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly BookDbContext _bookDbContext;
        private readonly IWebHostEnvironment _environment;

        public AuthorController(BookDbContext bookDbContext, IWebHostEnvironment environment)
        {
            _bookDbContext = bookDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _bookDbContext.Authors.ToListAsync();
            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

    }
}
