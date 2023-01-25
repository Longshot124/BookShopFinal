using BookShop.Areas.Admin.Models;
using BookShop.BLL.Extensions;
using BookShop.Data.Helpers;
using BookShop.Core.Entities;
using BookShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    public class PartnerController : BaseController
    {
        private readonly BookDbContext _bookDbContext;

        public PartnerController(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var partners = await _bookDbContext.Partners.ToListAsync();
            return View(partners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "You must choose photo");
                return View();
            }
            if (!model.Image.IsAllowedSize(5))
            {
                ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.PartnerPath);

            var partner = new Partner
            {
                ImageUrl = unicalName,
                Name = model.Name,
                PartnerUrl = model.PartnerUrl
                
            };

            await _bookDbContext.Partners.AddAsync(partner);
            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var partners = await _bookDbContext.Partners.FindAsync(id);

            if (partners == null) return NotFound();

            var partnerViewModel = new PartnerUpdateViewModel
            {
                ImageUrl = partners.ImageUrl,
                Id = partners.Id,
                Name = partners.Name,
                PartnerUrl = partners.PartnerUrl,
            };

            return View(partnerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, PartnerUpdateViewModel model)
        {
            if (id == null) return NotFound();

            var partners = await _bookDbContext.Partners.FindAsync(id);

            if (partners == null) return NotFound();
            if (partners.Id != id) return BadRequest();


            if (model.Image != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = partners.ImageUrl
                    });
                }
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "You must choose photo");
                    return View(new AuthorUpdateViewModel
                    {
                        ImageUrl = partners.ImageUrl
                    });
                }
                if (!model.Image.IsAllowedSize(5))
                {
                    ModelState.AddModelError("Image", "Image size is over 5MB, Please select less than 5 mb");
                    return View(model);
                }
                var unicalName = await model.Image.GenerateFile(Constants.PartnerPath);
                partners.ImageUrl = unicalName;
            }

            partners.Name = model.Name;
            partners.PartnerUrl = model.PartnerUrl;
            

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbPartner = await _bookDbContext.Partners.FindAsync(id);

            if (dbPartner == null) return NotFound();
            if (dbPartner.Id != id) return BadRequest();

            var unicalPath = Path.Combine(Constants.RootPath, "assets", "images", "partner", dbPartner.ImageUrl);

            if (System.IO.File.Exists(unicalPath))
                System.IO.File.Delete(unicalPath);

            _bookDbContext.Partners.Remove(dbPartner);

            await _bookDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
