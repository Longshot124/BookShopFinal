using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Author : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageUrl { get; set; } 
        public byte Age { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();

    }
}
