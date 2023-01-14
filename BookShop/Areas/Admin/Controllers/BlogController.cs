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
    public class BlogController : BaseController
    {
        private readonly BookDbContext _bookDbContext;

        public BlogController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _bookDbContext.Blogs.Where(b => !b.IsDeleted)
                .Include(b => b.BlogCategory)
                .ToListAsync();

            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            var blogCategories = await _bookDbContext.BlogCategories.Where(e => !e.IsDeleted).ToListAsync();
            var blogCategoryList = new List<SelectListItem>
            {
                new SelectListItem("Select Category" , "0")
            };
            
            blogCategories.ForEach(c => blogCategoryList.Add(new SelectListItem(c.Name, c.Id.ToString())));


            var model = new BlogCreateViewModel
            {
                BlogCategories = blogCategoryList,
                
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {

            var blogCategories = await _bookDbContext
                .BlogCategories
                .Where(e => !e.IsDeleted)
                .Include(e => e.Blogs)
                .ToListAsync();
            
            if (!ModelState.IsValid) return View(model);
            var blogCategoryList = new List<SelectListItem>
            {
                new SelectListItem("Category does't select","0")

            };
            
            blogCategories.ForEach(e => blogCategoryList.Add(new SelectListItem(e.Name, e.Id.ToString())));

            var blogViewModel = new BlogCreateViewModel()
            {
                BlogCategories = blogCategoryList,
                
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

            if (model.BlogCategoryId == 0)
            {
                ModelState.AddModelError("", "Category does't select");
                return View();
            }
            
            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            var newBlog = new Blog
            {
                ImageUrl = unicalName,
                BlogCategoryId = model.BlogCategoryId,
                Title = model.Title,
                Description = model.Description,
                Author = model.Author,
                Created = DateTime.Now,
            };

            await _bookDbContext.Blogs.AddAsync(newBlog);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            var blogCategory = await _bookDbContext.BlogCategories.Where(e => !e.IsDeleted).ToListAsync();
           

            if (blogCategory == null ) return NotFound();
            var blog = await _bookDbContext.Blogs
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.BlogCategory)
                .FirstOrDefaultAsync();
            if (blog == null) return NotFound();
            if (blog.Id != id) return NotFound();

            var selectBlogCategory = new List<SelectListItem>();
            

            var viewModel = new BlogUpdateViewModel
            {
                BlogCategories = selectBlogCategory,
                
            };

            if (!ModelState.IsValid) return View(viewModel);

            blogCategory.ForEach(e => selectBlogCategory.Add(new SelectListItem(e.Name, e.Id.ToString())));
            

            var blogUpdateModel = new BlogUpdateViewModel
            {
                Id = blog.Id,
                Author = blog.Author,
                Title = blog.Title,
                Description = blog.Description,
                ImageUrl = blog.ImageUrl,
                BlogCategories = selectBlogCategory,
                BlogCategoryId = blog.BlogCategoryId,
                

            };

            return View(blogUpdateModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var blogCategories = await _bookDbContext.BlogCategories.Where(e => !e.IsDeleted).ToListAsync();
            

            if (blogCategories == null ) return NotFound();

            var blog = await _bookDbContext.Blogs.Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.BlogCategory)
                
                .FirstOrDefaultAsync();

            if (blog == null) return NotFound();

            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new BlogUpdateViewModel
                    {
                        ImageUrl = model.ImageUrl,
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose image");
                    return View(new BookUpdateViewModel
                    {
                        ImageUrl = blog.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB!!!!");

                    return View(model);
                }

                var unicalPath = Path.Combine(Constants.BlogPath, blog.ImageUrl);

                if (System.IO.File.Exists(unicalPath))
                    System.IO.File.Delete(unicalPath);


                var unicalFile = await model.Image.GenerateFile(Constants.BlogPath);
                blog.ImageUrl = unicalFile;

            }

            var selectedBook = new BlogUpdateViewModel
            {
                BlogCategoryId = model.BlogCategoryId,
            };

            blog.Author = model.Author;
            blog.Description = model.Description;
            blog.Title = model.Title;
            blog.BlogCategoryId = model.BlogCategoryId;
            

            await _bookDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _bookDbContext.Blogs.FirstOrDefaultAsync(e => e.Id == id);

            if (blog == null) return NotFound();

            if (blog.Id != id) return BadRequest();
            var unicalPath = Path.Combine(Constants.BlogPath, "images", "blog", blog.ImageUrl);


            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Blogs.Remove(blog);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
