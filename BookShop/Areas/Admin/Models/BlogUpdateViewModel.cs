using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class BlogUpdateViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public List<SelectListItem>? BlogCategories { get; set; } = new();
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
