using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Partner : Entity
    {
        public string Name { get;set; }
        public string ImageUrl { get;set; }
        [Url]
        public string PartnerUrl { get; set; }
    }
}
