using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Data.Models;
using ProductsBase.Domain.Services.Communication;

namespace ProductsBase.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ListAllAsync();
        Task<Page<Product>> ListAllPagedAsync(int pageNumber, int pageSize);
        Task<ItemListResponse<Product>> ListByCategoryAsync(int categoryId);
        Task<ItemResponse<Page<Product>>> ListByCategoryPagedAsync(int categoryId, int pageNumber, int pageSize);
        Task<ItemResponse<Product>> SaveAsync(Product product);
        Task<ItemResponse<Product>> UpdateAsync(int id, Product product);
        Task<ItemResponse<Product>> DeleteAsync(int id);
    }
}