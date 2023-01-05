using BookShop.Areas.Admin.Models;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly BookDbContext _bookDbContext;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(BookDbContext bookDbContext, IWebHostEnvironment environment)
        {
            _bookDbContext = bookDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _bookDbContext.Categories.ToListAsync();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existCategory = await _bookDbContext.Categories.Where(c => c.IsDeleted).ToListAsync();

            if (existCategory.Any(c => c.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim()))) 
            {
                ModelState.AddModelError("Name", "This Category is already exist");
                return View();
            }
            var category = new Category
            {
                Name = model.Name,
            };

            await _bookDbContext.Categories.AddAsync(category);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _bookDbContext.Categories.FindAsync(id);
            if(category.Id != id) return BadRequest();

            var existCategory = new CategoryUpdateViewModel
            {
                Name = category.Name,
            };
            return View(existCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateViewModel model)
        {
            if(id==null) return NotFound();
            var categories = await _bookDbContext.Categories.FindAsync(id);
            if (categories == null) return NotFound();
            if (categories.Id != id) return BadRequest();

            var existName = await _bookDbContext.Categories.AnyAsync(c=>c.Name.
            ToLower().Trim()==model.Name.
            ToLower().Trim()&& c.Id !=id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Category is already exist!");
                return View(model);
            }
            categories.Name = model.Name;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _bookDbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();
            if (category.Id != id) return BadRequest();

            _bookDbContext.Categories.Remove(category);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
