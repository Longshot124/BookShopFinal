using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
	public class NewsController : BaseController
	{
        private readonly BookDbContext _bookDbContext;

        public NewsController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _bookDbContext.News.ToListAsync();
            return View(news);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);



            var news = new News
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now
            };

            await _bookDbContext.News.AddAsync(news);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var newss = await _bookDbContext.News.FindAsync(id);

            if (newss == null) return NotFound();
            var newsViewModel = new NewsUpdateViewModel
            {
                ImageUrl = newss.ImageUrl,
                Id = newss.Id,
                Title = newss.Title,
                Description = newss.Description,
                Created = DateTime.Now,
            };

            return View(newsViewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, NewsUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var newss = await _bookDbContext.News.FindAsync(id);
            if (newss == null) return NotFound();
            if (newss.Id != id) return BadRequest();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new NewsUpdateViewModel
                    {
                        ImageUrl = newss.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Şəkil seçməlisiniz");
                    return View(new NewsUpdateViewModel
                    {
                        ImageUrl = newss.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Şəkilin ölçüsü 5MB artıq olmamalıdır");
                    return View(model);
                }
                var unicalPath = await model.Image.GenerateFile(Constants.NewsPath);
                newss.ImageUrl = unicalPath;
            }

            newss.Description = model.Description;
            newss.Title = model.Title;
            newss.Created = model.Created;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var news = await _bookDbContext.News.FindAsync(id);

            if (news == null) return NotFound();

            if (news.ImageUrl == null) return NotFound();

            if (news.Id != id) return BadRequest();

            var newsPath = Path.Combine(Constants.NewsPath, "img", "news", news.ImageUrl);

            if (System.IO.File.Exists(newsPath))
                System.IO.File.Delete(newsPath);

            _bookDbContext.News.Remove(news);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
