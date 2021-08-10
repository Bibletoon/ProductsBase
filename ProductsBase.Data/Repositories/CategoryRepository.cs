using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Contexts;
using ProductsBase.Data.Models;
using ProductsBase.Data.Repositories.Interfaces;
using ProductsBase.Data.Utility.Extensions;

namespace ProductsBase.Data.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Category>> ListAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Page<Category>> ListAllPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Categories.PaginateAsync(pageNumber, pageSize,CancellationToken.None);
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }

        public void Remove(Category category)
        {
            _dbContext.Categories.Remove(category);
        }
    }
}