using BookShop.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Data.DAL
{
    public class BookDbContext : IdentityDbContext<AppUser>
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<FooterLogo> FooterLogos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListBook> WishListBook { get; set; } 
        public DbSet<Basket> Baskets { get; set; }  
        public DbSet<BasketBook> BasketBooks { get; set; }  
        public DbSet<About> Abouts { get; set; }
        public DbSet<OurMission> OurMissions { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(k => k.Key).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
