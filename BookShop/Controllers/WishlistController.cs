using BookShop.Core.Entities;
using BookShop.Data.DAL;
using BookShop.Data.Helpers;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Misc;

namespace BookShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly BookDbContext _bookDbContext;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(BookDbContext bookDbContext, UserManager<AppUser> userManager)
        {
            _bookDbContext = bookDbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<WishListViewModel> model = new();


            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var wishList = await _bookDbContext.WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListBooks)
                    .ThenInclude(x => x.Book)
                    .FirstOrDefaultAsync();

                if (wishList != null)
                {
                    foreach (var item in wishList.WishListBooks)
                    {
                        model.Add(new WishListViewModel
                        {
                            Id = item.BookId,
                            Name = item.Book.Name,
                            Price = item.Book.Price,
                            Offer = item.Book.Offer,
                            ImageUrl = item.Book.ImageUrl
                        });
                    }
                }
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var bookIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    foreach (var bookId in bookIdList)
                    {
                        var book = await _bookDbContext.Books
                            .Where(x => x.Id == bookId)

                            .FirstOrDefaultAsync();

                        model.Add(new WishListViewModel
                        {
                            Id = book.Id,
                            Name = book.Name,
                            Price = book.Price,
                            Offer = book.Offer,
                            ImageUrl = book.ImageUrl,

                        });
                    }
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWishlist(int? productId)
        {
            if (productId == null)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var existWishList = await _bookDbContext.WishList
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListBooks)
                    .FirstOrDefaultAsync();

                if (existWishList != null) 
                {
                    var createdWishList = new WishList
                    {
                        UserId = user.Id,
                        WishListBooks = new List<WishListBook>()
                    };

                    var existedProduct = await _bookDbContext.Books.FindAsync(productId);

                    if (existedProduct == null) return NotFound();

                    if (existWishList.WishListBooks.Any(w => w.BookId == existedProduct.Id))
                        return NoContent();

                    existWishList.WishListBooks.Add(new WishListBook
                    {
                        WishListId = createdWishList.Id,
                        BookId = existedProduct.Id
                    });

                    _bookDbContext.Update(existWishList);
                }
                else
                {
                    var createdWishList = new WishList
                    {
                        UserId = user.Id,
                        WishListBooks = new List<WishListBook>()
                    };

                    var wishListProducts = new List<WishListBook>();

                    var existedProduct = await _bookDbContext.Books.FindAsync(productId);

                    if (existedProduct == null) return NotFound();

                    wishListProducts.Add(new WishListBook
                    {
                        WishListId = createdWishList.Id,
                        BookId = existedProduct.Id
                    });

                    createdWishList.WishListBooks = wishListProducts;

                    await _bookDbContext.WishList.AddAsync(createdWishList);
                }

                await _bookDbContext.SaveChangesAsync();

            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    if (productIdList.Contains(productId.Value))
                        return NoContent();

                    productIdList.Add(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
                else
                {
                    var productIdListJson = JsonConvert.SerializeObject(new List<int> { productId.Value });

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
            }

            return NoContent();

        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductFromWishList(int? productId)
        {
            if (productId == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null) return BadRequest();

                var wishlist = await _bookDbContext.WishList.Where(x => x.UserId == user.Id)
                    .Include(x => x.WishListBooks).FirstOrDefaultAsync();

                var existProduct = await _bookDbContext.Books.FindAsync(productId);

                if(existProduct == null) return NotFound();

                var existWishlist = wishlist.WishListBooks.FirstOrDefault(x => x.BookId == existProduct.Id);

                wishlist.WishListBooks.Remove(existWishlist);

                _bookDbContext.Update(wishlist);

                await _bookDbContext.SaveChangesAsync();
            }
            else
            {
                if (Request.Cookies.TryGetValue(Constants.WISH_LIST_COOKIE_NAME, out var cookie))
                {
                    var productIdList = JsonConvert.DeserializeObject<List<int>>(cookie);

                    productIdList.Remove(productId.Value);

                    var productIdListJson = JsonConvert.SerializeObject(productIdList);

                    Response.Cookies.Append(Constants.WISH_LIST_COOKIE_NAME, productIdListJson);
                }
            }

            

            return NoContent();


        }
    }
}
