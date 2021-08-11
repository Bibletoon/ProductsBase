using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsBase.Data.Contexts;
using ProductsBase.Data.Models;
using ProductsBase.Data.Utility.Extensions;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public CategoryService(AppDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<CategoryService>();
        }

        public async Task<IEnumerable<Category>> ListAllAsync() => await _dbContext.Categories.ToListAsync();

        public async Task<Page<Category>> ListAllPagedAsync(int pageNumber, int pageSize) =>
            await _dbContext.Categories.PaginateAsync(pageNumber, pageSize);

        public async Task<ItemResponse<Category>> SaveAsync(Category category)
        {
            try
            {
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Category>(category);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Category>(errorMessage);
            }
        }

        public async Task<ItemResponse<Category>> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id);

            if (existingCategory is null)
            {
                return new ItemResponse<Category>("Category was not found");
            }

            existingCategory.Name = category.Name;

            try
            {
                _dbContext.Categories.Update(existingCategory);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Category>(existingCategory);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Category>(errorMessage);
            }
        }

        public async Task<ItemResponse<Category>> DeleteAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null)
            {
                return new ItemResponse<Category>("Can not find category");
            }


            try
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Category>(category);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Category>(errorMessage);
            }
        }
    }
}