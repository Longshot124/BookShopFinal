namespace BookShop.ViewModels
{
    public class WishListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Offer { get; set; }
        public decimal DiscountPrice => Price * (100 - Offer) / 100;
    }
}
