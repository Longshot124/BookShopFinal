using BookShop.Core.Entities;

namespace BookShop.ViewModels
{
	public class ShopViewModel
	{
        
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Book> Books { get; set; } = new List<Book>();


    }
}
