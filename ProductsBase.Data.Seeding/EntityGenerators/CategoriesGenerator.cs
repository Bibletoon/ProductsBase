using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Seeding.EntityFakers;
using ProductsBase.Domain.Models;

namespace ProductsBase.Data.Seeding.EntityGenerators
{
    public class CategoriesGenerator : IEntityGenerator
    {
        public List<Category> Categories { get; set; }

        private const int CategoriesCount = 20;
        
        public CategoriesGenerator()
        {
            Categories = CategoriesFaker.Instance.Value.CreateCategoryFaker().Generate(CategoriesCount);
        }
        
        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(Categories);
        }
    }
}