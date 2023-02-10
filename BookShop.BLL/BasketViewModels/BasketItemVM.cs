using BookShop.Core.Entities;

namespace BookShop.BLL.BasketViewModels
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
