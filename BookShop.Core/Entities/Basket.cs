using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Basket : Entity
    {
       
        public string UserId { get; set; }
        public List<BasketBook> BasketBooks { get; set; }
    }
}
