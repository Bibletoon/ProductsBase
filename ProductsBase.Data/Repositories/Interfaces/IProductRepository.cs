using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Data.Models;

namespace ProductsBase.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ListAllAsync();
        Task<Page<Product>> ListAllPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Product>> ListByCategoryAsync(Category category);
        Task<Page<Product>> ListByCategoryPagedAsync(Category category, int pageNumber, int pageSize);
        Task AddAsync(Product product);
        Task<Product> FindByIdAsync(int id);
        void Update(Product product);
        void Remove(Product product);
    }
}