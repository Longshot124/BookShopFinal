using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class WishListBook : Entity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int WishListId { get; set; }
        public WishList WishList { get; set; }
    }
}
