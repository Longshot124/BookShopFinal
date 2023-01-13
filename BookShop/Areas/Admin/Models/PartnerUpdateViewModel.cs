namespace BookShop.Areas.Admin.Models
{
    public class PartnerUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public string PartnerUrl { get; set; }
    }
}
