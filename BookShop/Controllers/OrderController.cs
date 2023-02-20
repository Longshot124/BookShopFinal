using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    public class OrderController : Controller
    {

        private readonly BookDbContext _bookDbContext;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(BookDbContext bookDbContext, UserManager<AppUser> userManager)
        {
            _bookDbContext = bookDbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderProduct()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("LogIn", "Account");

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is null) return BadRequest();

            var basket = await _bookDbContext
                    .Baskets
                    .Where(x => x.UserId == user.Id)
                    .Include(x=>x.BasketBooks)
                    .ThenInclude(x =>x.Book)
                    .FirstOrDefaultAsync();

            if (basket is null) return NotFound();

            _bookDbContext.Baskets.Remove(basket);

            var order = new Order
            {
                UserName = user.UserName,
                UserEmail = user.Email,
                UserId = user.Id,
                CreateTime = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (var product in basket.BasketBooks)
            {
                order.OrderItems.Add(new OrderItem
                {
                    BookId = product.BookId,
                    OrderId = order.Id,
                    
                    Count = product.Count,
                    Image = product.Book.ImageUrl
                });
            }

            await _bookDbContext.Orders.AddAsync(order);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
