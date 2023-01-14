using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class BlogCreateViewModel
    {
        public List<SelectListItem>? BlogCategories { get; set; }
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image{ get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
