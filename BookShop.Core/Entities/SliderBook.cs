using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class SliderBook : Entity
    {
        public Slider Slider { get; set; }
        public int SliderId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}
