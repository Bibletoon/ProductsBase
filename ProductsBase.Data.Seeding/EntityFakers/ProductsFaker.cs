using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductsBase.Domain.Models;

namespace ProductsBase.Data.Seeding.EntityFakers
{
    public class ProductsFaker
    {
        public static readonly Lazy<ProductsFaker> Instanse = new Lazy<ProductsFaker>();

        public Faker<Product> CreateProductsFaker(List<Category> categories)
        {
            return new Faker<Product>()
                   .RuleFor(p => p.Id, f => f.IndexVariable++ + 1)
                   .RuleFor(p => p.Name, f => f.Lorem.Word())
                   .RuleFor(p => p.QuantityInPackage, f => f.Random.UShort())
                   .RuleFor(p => p.UnitOfMeasurement, f => f.PickRandom<UnitOfMeasurement>())
                   .RuleFor(p => p.CategoryId, f => f.PickRandom(categories.Select(c => c.Id)));
        }
    }
}