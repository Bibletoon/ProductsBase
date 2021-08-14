using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Services.Communication;

namespace ProductsBase.Domain.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> ListAllAsync();
        Task<Page<Category>> ListAllPagedAsync(int pageNumber, int pageSize);
        Task<ItemResponse<Category>> SaveAsync(Category category);
        Task<ItemResponse<Category>> UpdateAsync(int id, Category category);
        Task<ItemResponse<Category>> DeleteAsync(int id);
    }
}