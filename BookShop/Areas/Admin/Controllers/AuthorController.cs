using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.BLL.Helpers;
using BookShop.Core.Entities;
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

        public async Task<IActionResult> Create(AuthorCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.AuthorPath);

            var author = new Author
            {
                ImageUrl = unicalName,
                Name = model.Name,
                Surname = model.Surname,
                Age = model.Age,
            };

            await _bookDbContext.Authors.AddAsync(author);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var authors = await _bookDbContext.Authors.FindAsync(id);

            if(authors == null) return NotFound();

            var authorModel = new AuthorUpdateViewModel
            {
                Id = authors.Id,
                Name = authors.Name,
                Surname = authors.Surname,
                Age = authors.Age,
                ImageUrl = authors.ImageUrl
            };
            return View(authorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, AuthorUpdateViewModel model)
        {
            if(id == null) return NotFound();

            var authors = await _bookDbContext.Authors.FindAsync(id);

            if (authors == null) return NotFound();
            if (authors.Id != id) return BadRequest();


            if (model.Image !=null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = authors.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose photo");
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = authors.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                    return View(model);
                }
                var unicalName = await model.Image.GenerateFile(Constants.AuthorPath);
                authors.ImageUrl = unicalName;
            }

            authors.Name = model.Name;
            authors.Surname = model.Surname;
            authors.Age = model.Age;
            
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbAuthor = await _bookDbContext.Authors.FindAsync(id);

            if (dbAuthor == null) return NotFound();
            if (dbAuthor.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.RootPath, "assets", "images", "author",dbAuthor.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Authors.Remove(dbAuthor);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}
