using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Seeding.EntityFakers;
using ProductsBase.Domain.Models;

namespace ProductsBase.Data.Seeding.EntityGenerators
{
    public class ProductsGenerator : IEntityGenerator
    {
        private const int ProductsCount = 20;

        public List<Product> Products { get; set; }
        
        public ProductsGenerator(List<Category> categories)
        {
            Products = ProductsFaker.Instanse.Value.CreateProductsFaker(categories).Generate(ProductsCount);
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(Products);
        }
    }
}