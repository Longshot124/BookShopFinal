using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Data.Helpers
{
    public class Constants
    {
        public static string RootPath;
        public static string AuthorPath;
        public static string BookPath;
        public static string SliderPath;
        public static string PartnerPath;
        public static string BlogPath;
        public static string FooterLogoPath;
        public static string UserPath;
        public static string AboutPath;
        public static string NewsPath;


        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public const string WISH_LIST_COOKIE_NAME = "WISHLIST";
        public const string BASKET_COOKIE_NAME = "BASKET";
    }
}
