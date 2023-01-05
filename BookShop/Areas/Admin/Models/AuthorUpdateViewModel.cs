namespace BookShop.Areas.Admin.Models
{
    public class AuthorUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile Image { get; set; }
        public byte Age { get; set; }
        public string? ImageUrl { get; set; } = String.Empty;

    }
}
