using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.Data.Helpers;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;

namespace BookShop.Areas.Admin.Controllers
{
    public class BookController : BaseController
    {
        private readonly BookDbContext _bookDbContext;

        public BookController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookDbContext.Books.Where(b => !b.IsDeleted)
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookLanguage)
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _bookDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Select Category" , "0")
            };
            var authors = await _bookDbContext.Authors.Where(e => !e.IsDeleted).ToListAsync();
            var authorsList = new List<SelectListItem>
            {
                new SelectListItem("Select Author" , "0")
            };
            var publishers = await _bookDbContext.Publishers.Where(e => !e.IsDeleted).ToListAsync();
            var publisherList = new List<SelectListItem>
            {
                new SelectListItem("Select Publisher" , "0")
            };
            var bookLanguages = await _bookDbContext.BookLanguages.Where(e => !e.IsDeleted).ToListAsync();
            var bookLanguagesList = new List<SelectListItem>
            {
                new SelectListItem("Select Language" , "0")
            };
            categories.ForEach(c => categoryList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            publishers.ForEach(c => publisherList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            authors.ForEach(c => authorsList.Add(new SelectListItem(c.Name, c.Id.ToString())));
            bookLanguages.ForEach(c => bookLanguagesList.Add(new SelectListItem(c.Name, c.Id.ToString())));


            var model = new BookCreateViewModel
            {
                Categories = categoryList,
                Authors = authorsList,
                Publishers = publisherList,
                BookLanguages=bookLanguagesList,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateViewModel model)
        {

            var categories = await _bookDbContext
                .Categories
                .Where(e => !e.IsDeleted)
                .Include(e => e.Books)
                .ToListAsync();
            var publishers = await _bookDbContext
                .Publishers.Where(e => !e.IsDeleted)
                .Include(e => e.Books)
                .ToListAsync();
            var authors = await _bookDbContext
                .Authors
                .Where(e => !e.IsDeleted)
                .Include(e => e.Books)
                .ToListAsync();
            var bookLanguages = await _bookDbContext
               .BookLanguages
               .Where(e => !e.IsDeleted)
               .Include(e => e.Books)
               .ToListAsync();
            if (!ModelState.IsValid) return View(model);
            var categoryList = new List<SelectListItem>
            {
                new SelectListItem("Category does't select","0")

            };
            var publisherList = new List<SelectListItem>
            {
                new SelectListItem("Publisher does't select","0")

            };
            var authorList = new List<SelectListItem>
            {
                new SelectListItem("Author does't select","0")

            };
            var bookLanguageList = new List<SelectListItem>
            {
                new SelectListItem("Language does't select","0")

            };
            categories.ForEach(e => categoryList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            publishers.ForEach(e => publisherList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            authors.ForEach(e => authorList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            bookLanguages.ForEach(e => bookLanguageList.Add(new SelectListItem(e.Name, e.Id.ToString())));


            var bookViewModel = new BookCreateViewModel()
            {
                Categories = categoryList,
                Publishers = publisherList,
                Authors = authorList,
                BookLanguages = bookLanguageList
            };

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose image");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5mb!!!");
                return View();
            }

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Category does't select");
                return View();
            }
            if (model.PublisherId == 0)
            {
                ModelState.AddModelError("", "Publisher does't select");
                return View();
            }
            if (model.AuthorId == 0)
            {
                ModelState.AddModelError("", "Author does't select");
                return View();
            }
            if (model.BookLanguageId == 0)
            {
                ModelState.AddModelError("", "Language does't select");
                return View();
            }
            var unicalName = await model.Image.GenerateFile(Constants.BookPath);

            var newBook = new Book
            {
                ImageUrl = unicalName,
                CategoryId = model.CategoryId,
                PublisherId = model.PublisherId,
                AuthorId = model.AuthorId,
                BookLanguageId = model.BookLanguageId,
                Name = model.Name,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                Offer = model.Offer,
                Description = model.Description,
                PageCount = model.PageCount,
                BookInfo = model.BookInfo,
            };

            await _bookDbContext.Books.AddAsync(newBook);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var category = await _bookDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var publisher = await _bookDbContext.Publishers.Where(e => !e.IsDeleted).ToListAsync();
            var author = await _bookDbContext.Authors.Where(e => !e.IsDeleted).ToListAsync();
            var bookLanguage = await _bookDbContext.BookLanguages.Where(e => !e.IsDeleted).ToListAsync();


            if (category == null || publisher == null || author == null || bookLanguage == null) return NotFound();
            var book = await _bookDbContext.Books
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.Category)
                .Include(e => e.Author)
                .Include(e => e.Publisher)
                .Include(e => e.BookLanguage)
                .FirstOrDefaultAsync();
            if (book == null) return NotFound();
            if (book.Id != id) return NotFound();

            var selectCategory = new List<SelectListItem>();
            var selectPublisher = new List<SelectListItem>();
            var selectAuthor = new List<SelectListItem>();
            var selectBookLanguage = new List<SelectListItem>();


            var viewModel = new BookUpdateViewModel
            {
                Categories = selectCategory,
                Publishers = selectPublisher,
                Authors = selectAuthor,
                BookLanguages = selectBookLanguage,
            };

            if (!ModelState.IsValid) return View(viewModel);

            category.ForEach(e => selectCategory.Add(new SelectListItem(e.Name, e.Id.ToString())));
            publisher.ForEach(e => selectPublisher.Add(new SelectListItem(e.Name, e.Id.ToString())));
            author.ForEach(e => selectAuthor.Add(new SelectListItem(e.Name, e.Id.ToString())));
            bookLanguage.ForEach(e => selectBookLanguage.Add(new SelectListItem(e.Name, e.Id.ToString())));


            var bookUpdateModel = new BookUpdateViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                Offer = book.Offer,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Categories = selectCategory,
                CategoryId = book.CategoryId,
                Publishers = selectPublisher,
                PublisherId = book.PublisherId,
                Authors = selectAuthor,
                AuthorId = book.AuthorId,
                BookLanguages = selectBookLanguage,
                BookLanguageId = book.BookLanguageId,
                BookInfo = book.BookInfo,

            };

            return View(bookUpdateModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,BookUpdateViewModel model)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var categories = await _bookDbContext.Categories.Where(e => !e.IsDeleted).ToListAsync();
            var publishers = await _bookDbContext.Publishers.Where(e => !e.IsDeleted).ToListAsync();
            var authors = await _bookDbContext.Publishers.Where(e => !e.IsDeleted).ToListAsync();
            var bookLanguages = await _bookDbContext.BookLanguages.Where(e => !e.IsDeleted).ToListAsync();


            if (categories == null || publishers == null || authors == null || bookLanguages == null) return NotFound();

            var book = await _bookDbContext.Books.Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.Category)
                .Include(e => e.Author)
                .Include(e => e.Publisher)
                .Include(e => e.BookLanguage)

                .FirstOrDefaultAsync();

            if(book== null) return NotFound();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new BookUpdateViewModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose image");
                    return View(new BookUpdateViewModel
                    {
                        ImageUrl = book.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB!!!!");

                    return View(model);
                }

                var unicalPath = Path.Combine(Constants.BookPath, book.ImageUrl);

                if (System.IO.File.Exists(unicalPath))
                    System.IO.File.Delete(unicalPath);


                var unicalFile = await model.Image.GenerateFile(Constants.BookPath);
                book.ImageUrl = unicalFile;

                

                
            }

            var selectedBook = new BookUpdateViewModel
            {
                CategoryId = model.CategoryId,
                PublisherId = model.PublisherId,
                AuthorId = model.AuthorId,
                BookLanguageId = model.BookLanguageId,
            };
            book.Name = model.Name;
            book.Description = model.Description;
            book.Offer = model.Offer;
            book.Price = model.Price;
            book.DiscountPrice = model.DiscountPrice;
            book.PageCount = model.PageCount;
            book.CategoryId = model.CategoryId;
            book.PublisherId = model.PublisherId;
            book.AuthorId = model.AuthorId;
            book.BookInfo = model.BookInfo;
            book.BookLanguageId = model.BookLanguageId;

            await _bookDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookDbContext.Books.FirstOrDefaultAsync(e => e.Id == id);

            if (book == null) return NotFound();

            if (book.Id != id) return BadRequest();
            var unicalPath = Path.Combine(Constants.BookPath,"images","books", book.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Books.Remove(book);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
