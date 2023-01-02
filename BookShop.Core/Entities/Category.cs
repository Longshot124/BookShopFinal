using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
