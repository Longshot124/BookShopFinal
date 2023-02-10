using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.BLL.BasketViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using BookShop.Data.Helpers;

namespace BookShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BookDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(BookDbContext bookDbContext, UserManager<AppUser> userManager)
        {
            _dbContext = bookDbContext;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> model = new();


            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var basket = await _dbContext
                    .Baskets
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.BasketBooks)
                    .ThenInclude(x => x.Book)
                    .FirstOrDefaultAsync();

                foreach (var item in basket.BasketBooks)
                {
                    var product = _dbContext.Books
                        .Where(p => p.Id == item.Book.Id && !p.IsDeleted)
                        
                        .FirstOrDefault();

                    model.Add(new BasketItemVM
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Count = item.Count,
                        ImageUrl = product.ImageUrl,
                    });
                }
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var bookList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    foreach (var item in bookList)
                    {
                        var product = _dbContext.Books
                            .Where(p => p.Id == item.Id && !p.IsDeleted)
                            .FirstOrDefault();

                        model.Add(new BasketItemVM
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Count = item.Count,
                            ImageUrl= product.ImageUrl,
                        });
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddToBasket(int? bookId)
        {
            if (bookId is null) return NotFound();

            var book = await _dbContext.Books
                   .Where(b => b.Id == bookId)
                   .FirstOrDefaultAsync();

            if (book is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var existBasket = await _dbContext
                   .Baskets
                   .Where(x => x.UserId == user.Id)
                   .Include(x => x.BasketBooks)
                   .FirstOrDefaultAsync();

                if (existBasket != null)
                {
                    var existBasketBook = existBasket.BasketBooks
                      .Where(x => x.BookId == book.Id)
                      .FirstOrDefault();

                    if (existBasketBook is not null)
                    {
                        existBasketBook.Count++;
                    }
                    else
                    {
                        var createdBasket = new Basket
                        {
                            UserId = user.Id,
                            BasketBooks = new List<BasketBook>()
                        };

                        existBasket.BasketBooks.Add(new BasketBook
                        {
                            BasketId = createdBasket.Id,
                            BookId = book.Id,
                            Count = 1
                        });
                    }

                    _dbContext.Update(existBasket);
                }
                else
                {
                    var createdBasket = new Basket
                    {
                        UserId = user.Id,
                        BasketBooks = new List<BasketBook>()
                    };

                    var basketProducts = new List<BasketBook>
                    {
                        new BasketBook
                        {
                            BasketId = createdBasket.Id,
                            BookId = book.Id,
                            Count = 1
                        }
                    };

                    createdBasket.BasketBooks = basketProducts;

                    await _dbContext.Baskets.AddAsync(createdBasket);
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var basket = Request.Cookies[Constants.BASKET_COOKIE_NAME];
                var basketItems = new List<BasketVM>();

                var basketItem = new BasketVM
                {
                    Id = book.Id,
                    Count = 1,
                };

                if (basket is not null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                    var existProduct = basketItems
                        .Where(x => x.Id == book.Id)
                        .FirstOrDefault();

                    if (existProduct is not null) existProduct.Count++;
                    else basketItems.Add(basketItem);
                }
                else
                {
                    basketItems.Add(basketItem);
                }

                Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, JsonConvert.SerializeObject(basketItems));
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProductQuality(int? bookId, int count)
        {
            if (bookId is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var basket = await _dbContext
                    .Baskets
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.BasketBooks)
                    .FirstOrDefaultAsync();

                var existBook = basket.BasketBooks.Where(book => book.BookId == bookId).FirstOrDefault();

                if (existBook is null) return NotFound();

                existBook.Count = count;

                _dbContext.Update(existBook);

                await _dbContext.SaveChangesAsync();

            }
            else
            {

                if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var productList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    var existProduct = productList.Where(x => x.Id == bookId).FirstOrDefault();

                    existProduct.Count = count;

                    var productIdListJson = JsonConvert.SerializeObject(productList);

                    Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductBasket(int? bookId)
        {
            if (bookId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var existBook = await _dbContext.Books.FindAsync(bookId);

                if (existBook == null) return NotFound();

                var existBasket = await _dbContext.Baskets
                    .Where(x => x.UserId == user.Id)
                   .Include(x => x.BasketBooks)
                   .FirstOrDefaultAsync();

                var existBasketBook = existBasket.BasketBooks
                    .FirstOrDefault(x => x.BookId == existBook.Id);

                existBasket.BasketBooks.Remove(existBasketBook);

                _dbContext.Update(existBasket);

                await _dbContext.SaveChangesAsync();

            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
                {
                    var productList = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);

                    var existProduct = productList.Where(x => x.Id == bookId).FirstOrDefault();

                    productList.Remove(existProduct);

                    var productIdListJson = JsonConvert.SerializeObject(productList);

                    Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();
        }
        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] == null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
    }
}
