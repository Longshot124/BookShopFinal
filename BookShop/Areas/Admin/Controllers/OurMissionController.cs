using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
	public class OurMissionController : BaseController
	{
		private readonly BookDbContext _bookDbContext;
		public OurMissionController(BookDbContext bookDbContext)
		{
			_bookDbContext = bookDbContext;
		}

        public async Task<IActionResult> Index()
        {
            var ourMission = await _bookDbContext.OurMissions.ToListAsync();
            return View(ourMission);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OurMissionCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
           
            var ourMission = new OurMission
            {
                Title = model.Title,
                Description = model.Description
            };

            await _bookDbContext.OurMissions.AddAsync(ourMission);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var ourMissions = await _bookDbContext.Abouts.FindAsync(id);

            if (ourMissions == null) return NotFound();

            var ourMissionModel = new OurMissionUpdateViewModel
            {
                Id = ourMissions.Id,
                Title = ourMissions.Title,
                Description = ourMissions.Description,
                
            };
            return View(ourMissionModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, OurMissionUpdateViewModel model)
        {
            if (id == null) return NotFound();
            var ourMissions = await _bookDbContext.OurMissions.FindAsync(id);
            if (ourMissions == null) return NotFound();
            if (ourMissions.Id != id) return BadRequest();

            var existName = await _bookDbContext.OurMissions.AnyAsync(c => c.Title.
            ToLower().Trim() == model.Title.
            ToLower().Trim() && c.Id != id);

            if (existName)
            {
                ModelState.AddModelError("Name", "This Category is already exist!");
                return View(model);
            }
            ourMissions.Title = model.Title;
            ourMissions.Description = model.Description;

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbOurMission = await _bookDbContext.OurMissions.FindAsync(id);

            if (dbOurMission == null) return NotFound();
            if (dbOurMission.Id != id) return BadRequest();



            _bookDbContext.OurMissions.Remove(dbOurMission);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
