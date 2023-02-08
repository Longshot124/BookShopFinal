using BookShop.Areas.Admin.Models;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.ViewComponents
{
	public class ContactMessageViewComponent : ViewComponent
	{
		private readonly BookDbContext _bookDbContext;

		public ContactMessageViewComponent(BookDbContext bookDbContext)
		{
			_bookDbContext = bookDbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var messages = await _bookDbContext.ContactMessages.ToListAsync();

			var isAllRead = messages.All(x => x.IsRead);

			return View(new ContactMessageViewModel
			{
				ContactMessages=messages,
				IsAllRead=isAllRead,
			});

		}
	}
}
