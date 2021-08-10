using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Contexts;
using ProductsBase.Data.Models;
using ProductsBase.Data.Repositories.Interfaces;
using ProductsBase.Data.Utility.Extensions;

namespace ProductsBase.Data.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> ListAllAsync()
        {
           return await _dbContext.Products.Include(p=>p.Category).ToListAsync();
        }

        public async Task<Page<Product>> ListAllPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Products.Include(p => p.Category).PaginateAsync(pageNumber, pageSize, CancellationToken.None);
        }

        public async Task<IEnumerable<Product>> ListByCategoryAsync(Category category)
        {
            return await _dbContext.Products.Include(p => p.Category).Where(p => p.Category == category).ToListAsync();
        }

        public async Task<Page<Product>> ListByCategoryPagedAsync(Category category, int pageNumber, int pageSize)
        {
            return await _dbContext.Products.Include(p => p.Category).Where(p=>p.Category==category).PaginateAsync(pageNumber, pageSize, CancellationToken.None);
        }

        public async Task AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public void Update(Product product)
        {
            _dbContext.Update(product);
        }

        public void Remove(Product product)
        {
            _dbContext.Remove(product);
        }
    }
}