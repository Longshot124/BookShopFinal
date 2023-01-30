using BookShop.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class BookUpdateViewModel
    {
        public int Id { get; set; } 
        public string? ImageUrl { get; set; } 
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Authors { get; set; } = new();
        public List<SelectListItem> Publishers { get; set; } = new();
        public List<SelectListItem> BookLanguages { get; set; } = new();
        public IFormFile? Image { get; set; }
        public string Name { get; set; }
        public string BookInfo { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public byte Offer { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; } 
        public int PublisherId { get; set; } 
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int BookLanguageId { get; set; }
       
    }
}
