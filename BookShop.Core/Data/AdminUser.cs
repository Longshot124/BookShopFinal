using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Data
{
    public class AdminUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Adress { get; set; }
        public string EmailConfirmationToken { get; set; }
        public byte Age { get; set; }
        public string Gender { get; set; }

    }
}
