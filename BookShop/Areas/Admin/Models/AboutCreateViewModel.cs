namespace BookShop.Areas.Admin.Models
{
    public class AboutCreateViewModel
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
