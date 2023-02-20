namespace BookShop.Areas.Admin.Models
{
    public class AboutUpdateViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
