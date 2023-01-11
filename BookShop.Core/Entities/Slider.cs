using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Slider : Entity
    {
        public string ImageUrl { get; set; }
        public string SliderName { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }


    }
}
