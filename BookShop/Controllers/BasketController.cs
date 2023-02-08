using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.BLL.BasketViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BookDbContext _bookDbContext;

        public BasketController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Book book = await _bookDbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if(book == null) return NotFound();
            string basketStr = HttpContext.Request.Cookies["Basket"];



            BasketVM basket;
            if (string.IsNullOrEmpty(basketStr))
            {
                basket = new BasketVM();
                BasketCookieItemVM cookieItemVM = new BasketCookieItemVM
                {
                    Id = book.Id,
                    Quantity = 1,
                };
                basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                basket.BasketCookieItemVMs.Add(cookieItemVM);
                basket.TotalPrice = book.DiscountPrice;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                BasketCookieItemVM existed = basket.BasketCookieItemVMs.Find(b => b.Id == id);
                if (existed==null)
                {
                    
                    BasketCookieItemVM cookieItemVM = new BasketCookieItemVM
                    {
                        Id = book.Id,
                        Quantity = 1,
                    };
                    basket.BasketCookieItemVMs.Add(cookieItemVM);
                    basket.TotalPrice += book.DiscountPrice;

                }
                else
                {
                    basket.TotalPrice += book.DiscountPrice;
                    existed.Quantity++;
                }
            }
            

            basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket", basketStr);

            return RedirectToAction("Shop","Index");
        }

        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] == null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
    }
}
