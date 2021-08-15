using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Seeding.EntityGenerators;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Security.Hashing;

namespace ProductsBase.Data.Seeding
{
    public class DatabaseSeeder
    {
        private IPasswordHasher _passwordHasher;
        
        private readonly List<IEntityGenerator> _generators = new List<IEntityGenerator>();

        public DatabaseSeeder(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            var categoriesGenerator = new CategoriesGenerator();
            var productsGenerator = new ProductsGenerator(categoriesGenerator.Categories);
            
            _generators.AddRange(new List<IEntityGenerator>()
            {
                categoriesGenerator,
                productsGenerator
            });
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

            _generators.ForEach(g=>g.Seed(modelBuilder));
        }
    }
}