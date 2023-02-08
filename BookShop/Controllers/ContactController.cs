using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactMessage = BookShop.Core.Entities.ContactMessage;

namespace BookShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly BookDbContext _bookDbContext;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(BookDbContext bookDbContext,UserManager<AppUser> userManager)
        {
            _bookDbContext = bookDbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ContactViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                model.ContactMessage = new ViewModels.ContactMessageViewModel
                {
                    Name = user.UserName,
                    Email = user.Email,
                };
            }

            return View(model);
        }
        
        public async Task<IActionResult> AddMessage(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(viewName: nameof(Index), model);
            }
            var message = new ContactMessage
            {
                Name = model.ContactMessage.Name,
                Email = model.ContactMessage.Email,
                Subject = model.ContactMessage.Subject,
                Message = model.ContactMessage.Message
            };

            await _bookDbContext.ContactMessages.AddAsync(message);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
