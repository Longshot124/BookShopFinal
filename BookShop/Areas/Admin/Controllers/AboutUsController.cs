using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class AboutUsController : BaseController
    {
        private readonly BookDbContext _bookDbContext;

        public AboutUsController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _bookDbContext.Abouts.ToListAsync();
            return View(about);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateViewModel model)
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

            var unicalName = await model.Image.GenerateFile(Constants.AboutPath);

            var about = new About
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description
            };

            await _bookDbContext.Abouts.AddAsync(about);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var abouts = await _bookDbContext.Abouts.FindAsync(id);

            if (abouts == null) return NotFound();

            var aboutModel = new AboutUpdateViewModel
            {
                Id = abouts.Id,
                Title = abouts.Title,
                Description = abouts.Description,
                ImageUrl = abouts.ImageUrl
            };
            return View(aboutModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, AboutUpdateViewModel model)
        {
            if (id == null) return NotFound();

            var abouts = await _bookDbContext.Abouts.FindAsync(id);

            if (abouts == null) return NotFound();
            if (abouts.Id != id) return BadRequest();


            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = abouts.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose photo");
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = abouts.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                    return View(model);
                }
                var unicalName = await model.Image.GenerateFile(Constants.AboutPath);
                abouts.ImageUrl = unicalName;
            }

            abouts.Title = model.Title;
            abouts.Description = model.Description;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbAbout = await _bookDbContext.Abouts.FindAsync(id);

            if (dbAbout == null) return NotFound();
            if (dbAbout.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.RootPath, "assets", "images", "about", dbAbout.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Abouts.Remove(dbAbout);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }


}
