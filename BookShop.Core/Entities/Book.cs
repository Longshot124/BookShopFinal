using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class Book : Entity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public byte Offer { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string BookInfo { get; set; }
        public int PublisherId { get;set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int BookLanguageId { get; set; }
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }  
        public BookLanguage BookLanguage { get; set; }
    }
}
