using BookShop.Areas.Admin.Models;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
	public class BookLanguageController : BaseController
	{
		private readonly BookDbContext _bookDbContext;
		public BookLanguageController(BookDbContext bookDbContext)
		{
			_bookDbContext = bookDbContext;
		}
        public async Task<IActionResult> Index()
        {
            var bookLanguage = await _bookDbContext.BookLanguages.ToListAsync();
            return View(bookLanguage);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(BookLanguageCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existBookLanguage = await _bookDbContext.BookLanguages.Where(c => c.IsDeleted).ToListAsync();

            if (existBookLanguage.Any(c => c.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name","This Language is already exist");
                return View();
            }
            var bookLanguage = new BookLanguage
            {
                Name = model.Name,
            };

            await _bookDbContext.BookLanguages.AddAsync(bookLanguage);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var bookLanguage = await _bookDbContext.BookLanguages.FindAsync(id);
            if (bookLanguage.Id != id) return BadRequest();

            var existBookLanguage = new BookLanguageUpdateViewModel
            {
                Name = bookLanguage.Name,
            };
            return View(existBookLanguage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BookLanguageUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var bookLanguages = await _bookDbContext.BookLanguages.FindAsync(id);
            if (bookLanguages == null) return NotFound();
            if (bookLanguages.Id != id) return BadRequest();

            var existName = await _bookDbContext.BookLanguages.AnyAsync(c => c.Name.
            ToLower().Trim() == model.Name.
            ToLower().Trim() && c.Id != id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Language is already exist!");
                return View(model);
            }
            bookLanguages.Name = model.Name;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bookLanguage = await _bookDbContext.BookLanguages.FindAsync(id);

            if (bookLanguage == null) return NotFound();
            if (bookLanguage.Id != id) return BadRequest();

            _bookDbContext.BookLanguages.Remove(bookLanguage);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }

}

