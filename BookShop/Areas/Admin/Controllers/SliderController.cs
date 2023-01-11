using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.BLL.Helpers;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        private readonly BookDbContext _bookDbContext;
        public SliderController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        

        public async Task<IActionResult> Index()
        {
            var sliders = await _bookDbContext
                .Sliders
                .Where(s => !s.IsDeleted)
                .Include(e => e.Book)
                .OrderByDescending(e=>e.Id)
                .ToListAsync();

            return View(sliders);
        }

        public async Task<IActionResult> Create()
        {
            var books = await _bookDbContext.Books.Where(e => !e.IsDeleted).ToListAsync();
            var bookList = new List<SelectListItem>
            {
                new SelectListItem("Choose book" , "0")
            };

            books.ForEach(e => bookList.Add(new SelectListItem(e.Name, e.Id.ToString())));
            var model = new SliderCreateViewModel
            {
                Books = bookList
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SliderCreateViewModel model)
        {
            var books = await _bookDbContext
                .Sliders
                .Where(e => !e.IsDeleted)
                .Include(e => e.Book)
                .ToListAsync();
            if (!ModelState.IsValid) return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You Must Choose Photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Photo size is over 5MB, Please select something else");
                return View();
            }
            if (model.BookId == 0)
            {
                ModelState.AddModelError("", "You must choose item");
                return View();
            }
            var unicalFile = await model.Image.GenerateFile(Constants.SliderPath);

            var newSlider = new Slider
            {
                ImageUrl = unicalFile,
                SliderName = model.SliderName,
                BookId = model.BookId,
                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,
            };

            await _bookDbContext.Sliders.AddAsync(newSlider);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var book = await _bookDbContext.Books.Where(e => !e.IsDeleted).ToListAsync();
            if (book == null) return NotFound();
            var slider = await _bookDbContext.Sliders
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.Book)
                .FirstOrDefaultAsync();
            if(slider == null) return NotFound();
            if (slider.Id != id) return NotFound();

            var selectedBook = new List<SelectListItem>();

            var viewModel = new SliderUpdateViewModel
            {
                Books = selectedBook
            };

            if (!ModelState.IsValid) return View(viewModel);

            book.ForEach(e => selectedBook.Add(new SelectListItem(e.Name, e.Id.ToString())));

            var sliderUpdateViewModel = new SliderUpdateViewModel
            {
                Id = slider.Id,
                ImageUrl = slider.ImageUrl,
                SliderName = slider.SliderName,
                ButtonText = slider.ButtonText,
                ButtonUrl = slider.ButtonUrl,
                Books = selectedBook,
                BookId = slider.BookId
            };

            return View(sliderUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id,SliderUpdateViewModel model)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View();
            var books = await _bookDbContext.Books.Where(e => e.IsDeleted).ToListAsync();
            if (books == null) return NotFound();
            var slider = await _bookDbContext.Sliders.Where(e => !e.IsDeleted && e.Id == id).Include(e => e.Book).FirstOrDefaultAsync();
            if (slider == null) return NotFound();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new SliderUpdateViewModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose an image");
                    return View(new SliderUpdateViewModel
                    {
                        ImageUrl = slider.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image's size is over 5MB!! Please select something else");

                    return View(model);
                }

                var unicalPath = Path.Combine(Constants.SliderPath, slider.ImageUrl);

                if (System.IO.File.Exists(unicalPath))
                    System.IO.File.Delete(unicalPath);


                

            }
            var unicalFile = await model.Image.GenerateFile(Constants.SliderPath);
            slider.ImageUrl = unicalFile;

            var selectedBook = new SliderUpdateViewModel
            {
                BookId = model.BookId
            };
            slider.SliderName = model.SliderName;
            slider.ButtonUrl = model.ButtonUrl;
            slider.ButtonText = model.ButtonText;
            slider.BookId = model.BookId;

            await _bookDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _bookDbContext.Sliders.FirstOrDefaultAsync(e => e.Id == id);

            if (slider == null) return NotFound();

            if (slider.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.SliderPath, "images", "sliders", slider.ImageUrl);



            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Sliders.Remove(slider);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
