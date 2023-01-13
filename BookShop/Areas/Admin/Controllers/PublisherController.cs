using BookShop.Areas.Admin.Models;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class PublisherController : BaseController
    {
        private readonly BookDbContext _bookDbContext;

        public PublisherController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var publisher = await _bookDbContext.Publishers.ToListAsync();
            return View(publisher);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existPublisher = await _bookDbContext.Publishers.Where(p => p.IsDeleted).ToListAsync();
            if (existPublisher.Any(p => p.Name.ToLower().Trim().Equals(model.Name.ToLower().Trim())))
            {
                ModelState.AddModelError("Name", "This publisher allready exist");
                return View(model);
            }
            var publisher = new Publisher
            {
                Name = model.Name,
                Adress = model.Adress,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            await _bookDbContext.Publishers.AddAsync(publisher);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var publisher = await _bookDbContext.Publishers.FindAsync(id);
            if (publisher.Id != id) return BadRequest();

            var existPublisher = new PublisherUpdateViewModel
            {
                Name = publisher.Name,
                Adress = publisher.Adress,
                Email = publisher.Email,
                PhoneNumber = publisher.PhoneNumber,
            };
            return View(existPublisher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, PublisherUpdateViewModel model)
        {
            if(id==null) return NotFound();
            var publishers = await _bookDbContext.Publishers.FindAsync(id);
            if (publishers == null) return NotFound();
            if(publishers.Id != id) return BadRequest();

            var ExistName = await _bookDbContext.Publishers.AnyAsync(
                p=>p.Name.ToLower().Trim()==
                model.Name .ToLower().Trim()&& 
                p.Id !=id);

            if (ExistName)
            {
                ModelState.AddModelError("Name", "This Publisher is already exist!!!");
                return View(model);
            }
            publishers.Name = model.Name;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var publisher = await _bookDbContext.Publishers.FindAsync(id);

            if (publisher == null) return NotFound();
            if (publisher.Id != id) return BadRequest();

            _bookDbContext.Publishers.Remove(publisher);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
