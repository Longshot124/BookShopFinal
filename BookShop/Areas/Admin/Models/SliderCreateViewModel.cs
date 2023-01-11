using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Models
{
    public class SliderCreateViewModel
    {
        public List<SelectListItem>? Books { get; set; }
        public IFormFile Image { get; set; }
        public int BookId { get; set; } 
        public string SliderName { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }
    }
}
