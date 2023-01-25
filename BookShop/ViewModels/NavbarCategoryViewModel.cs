using BookShop.Core.Entities;

namespace BookShop.ViewModels
{
	public class NavbarCategoryViewModel
	{
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
