using BookShop.Core.Entities;

namespace BookShop.Areas.Admin.Models
{
    public class ContactMessageViewModel
    {
        public List<ContactMessage> ContactMessages { get; set; }
        public bool IsAllRead { get; set; }
    }
}
