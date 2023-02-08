using BookShop.Core.Entities;

namespace BookShop.BLL.BasketViewModels
{
    public class BasketItemVM
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
