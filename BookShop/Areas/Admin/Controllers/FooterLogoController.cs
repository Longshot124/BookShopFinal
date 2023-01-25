using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
	public class FooterLogoController : BaseController
	{
		private readonly BookDbContext _bookDbContext;

		public FooterLogoController(BookDbContext bookDbContext)
		{
			_bookDbContext = bookDbContext;
		}

		public async Task<IActionResult> Index()
		{
			var footerLogos = await _bookDbContext.FooterLogos.ToListAsync();
			return View(footerLogos);
		}


	}
}
