using Microsoft.EntityFrameworkCore;

namespace ProductsBase.Data.Seeding.EntityGenerators
{
    public interface IEntityGenerator
    {
        void Seed(ModelBuilder modelBuilder);
    }
}