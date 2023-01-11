using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class SliderUpdateViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public List<SelectListItem>? Books { get; set; } = new();
        public IFormFile? Image { get; set; }
        public int BookId { get; set; }
        public string SliderName { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
    }
}
