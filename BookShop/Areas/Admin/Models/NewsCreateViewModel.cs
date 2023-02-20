namespace BookShop.Areas.Admin.Models
{
    public class NewsCreateViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public IFormFile Image { get; set; }
    }
}
