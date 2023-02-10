using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class BasketBook : Entity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int Count { get; set; }
    }
}
