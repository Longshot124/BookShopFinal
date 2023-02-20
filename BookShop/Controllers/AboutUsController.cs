using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public AboutUsController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            //var about = _bookDbContext.Abouts.FirstOrDefault();
            About about = await _bookDbContext.Abouts.Where(e => !e.IsDeleted).FirstOrDefaultAsync();
            List<OurMission> ourMissions = await _bookDbContext.OurMissions.Where(e => !e.IsDeleted).ToListAsync();
            AboutViewModel aboutViewModel = new AboutViewModel
            {
                About = about,
                OurMissions = ourMissions
                
            };
            return View(aboutViewModel);
        }
    }
}
