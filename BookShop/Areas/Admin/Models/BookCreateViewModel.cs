using BookShop.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class BookCreateViewModel
    {
        public List<SelectListItem>? Categories { get; set; } 
        public List<SelectListItem>? Authors { get; set; } 
        public List<SelectListItem>? Publishers { get; set; }
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public byte Offer { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public int PublisherId { get; set; } 
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

    }
}
