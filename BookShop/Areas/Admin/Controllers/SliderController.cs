using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        private readonly BookDbContext _bookDbContext;
        private readonly IWebHostEnvironment _environment;
        public SliderController(BookDbContext bookDbContext, IWebHostEnvironment environment)
        {
            _bookDbContext = bookDbContext;
            _environment = environment;
        }
        

        public async Task<IActionResult> Index()
        {
            var sliders = await _bookDbContext.Sliders.ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}

    }
}
