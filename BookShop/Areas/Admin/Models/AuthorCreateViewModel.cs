namespace BookShop.Areas.Admin.Models
{
    public class AuthorCreateViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile Image { get; set; }
        public byte Age { get; set; }

    }
}
