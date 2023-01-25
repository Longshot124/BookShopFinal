using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
	public class FooterLogo : Entity
	{
		public string FooterLogoUrl { get; set; }
		public string Description { get; set; }
        public string? FacebookLink { get; set; }
        public string? YoutubetLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? InstagramLink { get; set; }

    }
}
