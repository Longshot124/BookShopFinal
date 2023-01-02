using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Slider : Entity
    {
       public string SliderName { get; set; }
       public string Title { get; set; }
       public string SubTitle { get; set; }
       public int Price { get; set; }
       public int OldPrice { get; set; }
       public string ButtonUrl { get; set; }

    }
}
