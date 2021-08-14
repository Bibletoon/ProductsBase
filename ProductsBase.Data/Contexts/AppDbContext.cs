using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProdictsBase.Data.Seeding;
using ProductsBase.Domain.Models;

namespace ProductsBase.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        private DatabaseSeeder _seeder;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, DatabaseSeeder seeder) : base(options) 
        {
            _seeder = seeder;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Roles)
                        .WithMany(r => r.Users)
                        .UsingEntity<Dictionary<string,object>>("UsersRoles",
                            r=> r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                            l=> l.HasOne<User>().WithMany().HasForeignKey("UserId"));

            modelBuilder.Entity<Category>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Category>().HasMany(p => p.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.QuantityInPackage).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.UnitOfMeasurement).IsRequired();
            
            Seed(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            _seeder.Seed(modelBuilder);
        }
    }
}