using BookShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BLL.BasketViewModels
{
    public class LayoutBasketViewModel
    {
        public List<Book> Books { get; set; }
        public int TotalPrice { get; set; }
    }
}
