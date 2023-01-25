using System.ComponentModel.DataAnnotations;

namespace BookShop.Areas.Admin.Models
{
	public class FooterLogoCreateViewModel
	{
        public IFormFile LogoImage { get; set; }
        
        public string Description { get; set; }
        [DataType(DataType.Url)]
        public string? FacebookLink { get; set; }
        [DataType(DataType.Url)]
        public string? YoutubetLink { get; set; }
        [DataType(DataType.Url)]
        public string? LinkedinLink { get; set; }
        [DataType(DataType.Url)]
        public string? InstagramLink { get; set; }
    }
}
