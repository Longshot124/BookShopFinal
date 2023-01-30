using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Entities
{
    public class AppUser:IdentityUser
    {
        public string? FisrtName { get; set; }
        public string? LastName { get; set; }
        public byte Age { get; set; }
        public string Gender { get; set; }
        public string? ImageUrl { get; set; }
        public string Adress { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}
