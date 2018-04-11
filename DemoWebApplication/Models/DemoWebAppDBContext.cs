using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class DemoWebAppDBContext : DbContext
    {
        public DemoWebAppDBContext() : base("DemoWebAppDBContext") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UsersRole> UsersRoles { get; set; }

        public DbSet<HomeBanner> HomeBanners { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}