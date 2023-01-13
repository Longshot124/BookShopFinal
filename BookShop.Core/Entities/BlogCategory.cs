using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class BlogCategory : Entity
    {
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>();

    }
}
