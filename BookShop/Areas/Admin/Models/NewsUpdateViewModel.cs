namespace BookShop.Areas.Admin.Models
{
    public class NewsUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; } 
        public DateTime Created { get; set; }
        public IFormFile Image { get; set; }
    }
}
