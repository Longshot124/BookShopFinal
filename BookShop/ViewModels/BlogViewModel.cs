using BookShop.Core.Entities;

namespace BookShop.ViewModels
{
    public class BlogViewModel
    {
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();
    }
}
