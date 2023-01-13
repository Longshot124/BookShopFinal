using BookShop.Areas.Admin.Models;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class BlogCategoryController : BaseController
    {
        public readonly BookDbContext _bookDbContext;

        public BlogCategoryController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var blogCategory = await _bookDbContext.BlogCategories.ToListAsync();
            return View(blogCategory);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existBlogCategory = await _bookDbContext.BlogCategories.Where(p => p.IsDeleted).ToListAsync();
            if (existBlogCategory.Any(p => p.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "This Blog category allready exist");
                return View(model);
            }
            var blogCategory = new BlogCategory
            {
                Name = model.Name,
                
            };

            await _bookDbContext.BlogCategories.AddAsync(blogCategory);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var blogCategory = await _bookDbContext.BlogCategories.FindAsync(id);
            if (blogCategory.Id != id) return BadRequest();

            var existBlogCategory = new BlogCategoryUpdateViewModel
            {
                Name = blogCategory.Name,
               
            };
            return View(existBlogCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogCategoryUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var blogCategories = await _bookDbContext.BlogCategories.FindAsync(id);
            if (blogCategories == null) return NotFound();
            if (blogCategories.Id != id) return BadRequest();

            var ExistName = await _bookDbContext.BlogCategories.AnyAsync(
                p => p.Name.ToLower().Trim() ==
                model.Name.ToLower().Trim() &&
                p.Id != id);

            if (ExistName)
            {
                ModelState.AddModelError("Name", "This Blog category is already exist!!!");
                return View(model);
            }
            blogCategories.Name = model.Name;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blogCategory = await _bookDbContext.BlogCategories.FindAsync(id);

            if (blogCategory == null) return NotFound();
            if (blogCategory.Id != id) return BadRequest();

            _bookDbContext.BlogCategories.Remove(blogCategory);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
