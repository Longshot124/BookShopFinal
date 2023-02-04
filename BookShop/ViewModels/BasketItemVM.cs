using BookShop.Core.Entities;

namespace BookShop.ViewModels
{
    public class BasketItemVM
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
