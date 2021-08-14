using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Security.Hashing;

namespace ProductsBase.Data.Seeding
{
    public class DatabaseSeeder
    {
        private IPasswordHasher _passwordHasher;

        public DatabaseSeeder(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            var roles = new List<Role>
            {
                new Role { Id=1, Name = ApplicationRole.Administrator.ToString() },
                new Role { Id=2, Name = ApplicationRole.Common.ToString() }
            };
            
            var users = new List<User>
            {
                new User { Id=1, Email = "admin@admin.com", Password = _passwordHasher.HashPassword("12345678") },
                new User { Id=2, Email = "common@common.com", Password = _passwordHasher.HashPassword("12345678") },
            };

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Role>().HasData(roles);

            modelBuilder.Entity("UsersRoles").HasData(
                new { RoleId = 1, UserId = 1 },
                new { RoleId = 2, UserId = 2 }
            );

            modelBuilder.Entity<Category>().HasData
            (
                new Category { Id = 100, Name = "Fruits and Vegetables" }, // Id set manually due to in-memory provider
                new Category { Id = 101, Name = "Dairy" }
            );
            
            modelBuilder.Entity<Product>().HasData
            (
                new Product
                {
                    Id = 100,
                    Name = "Apple",
                    QuantityInPackage = 1,
                    UnitOfMeasurement = UnitOfMeasurement.Unity,
                    CategoryId = 100
                },
                new Product
                {
                    Id = 101,
                    Name = "Milk",
                    QuantityInPackage = 2,
                    UnitOfMeasurement = UnitOfMeasurement.Liter,
                    CategoryId = 101,
                }
            );
        }
    }
}