using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Publisher : Entity
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
