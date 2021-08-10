using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Data.Models;

namespace ProductsBase.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> ListAllAsync();
        Task<Page<Category>> ListAllPagedAsync(int pageNumber, int pageSize);
        Task AddAsync(Category category);
        Task<Category> FindByIdAsync(int id);
        void Update(Category category);
        void Remove(Category category);
    }
}