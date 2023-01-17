using BookShop.Core.Entities;

namespace BookShop.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Publisher> Publishers { get; set; } = new List<Publisher>();
        public List<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();
        public List<Partner> Partners { get; set; } = new List<Partner>();
    }
}
