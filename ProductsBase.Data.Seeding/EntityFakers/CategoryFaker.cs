using System;
using Bogus;
using ProductsBase.Domain.Models;

namespace ProductsBase.Data.Seeding.EntityFakers
{
    public class CategoriesFaker
    {
        public static readonly Lazy<CategoriesFaker> Instance = new Lazy<CategoriesFaker>();

        public Faker<Category> CreateCategoryFaker()
        {
            return new Faker<Category>()
                   .RuleFor(c => c.Id, f => f.IndexVariable++ + 1)
                   .RuleFor(c => c.Name, f => f.Lorem.Slug(2));
        }
    }
}