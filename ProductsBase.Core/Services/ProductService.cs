using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ProductsBase.Data.Contexts;
using ProductsBase.Data.Utility.Extensions;
using ProductsBase.Domain.Models;

namespace ProductsBase.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        private readonly ILogger _logger;

        public ProductService(AppDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<ProductService>();
        }


        public async Task<IEnumerable<Product>> ListAllAsync() => await _dbContext.Products.ToListAsync();

        public async Task<Page<Product>> ListAllPagedAsync(int pageNumber, int pageSize) =>
            await _dbContext.Products.PaginateAsync(pageNumber, pageSize);

        public async Task<ItemListResponse<Product>> ListByCategoryAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return new ItemListResponse<Product>("Invalid category");
            }

            var products = await _dbContext.Products.Where(p=>p.Category == category).ToListAsync();
            return new ItemListResponse<Product>(products);
        }

        public async Task<ItemResponse<Page<Product>>> ListByCategoryPagedAsync(int categoryId,
            int pageNumber,
            int pageSize)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category is null)
            {
                return new ItemResponse<Page<Product>>("Invalid category");
            }

            var products = await _dbContext.Products.Where(p=>p.Category == category)
                                           .PaginateAsync(pageNumber, pageSize);
            return new ItemResponse<Page<Product>>(products);
        }

        public async Task<ItemResponse<Product>> SaveAsync(Product product)
        {
            var category = await _dbContext.Categories.FindAsync(product.CategoryId);
            if (category is null)
            {
                return new ItemResponse<Product>("Invalid category");
            }
            
            try
            {
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Product>(product);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Product>(errorMessage);
            }
        }

        public async Task<ItemResponse<Product>> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _dbContext.Products.FindAsync(id);

            if (existingProduct is null)
            {
                return new ItemResponse<Product>("Product was not found");
            }

            var newCategory = await _dbContext.Categories.FindAsync(product.CategoryId);

            if (newCategory is null)
            {
                return new ItemResponse<Product>("Invalid category");
            }

            existingProduct.Name = product.Name;
            existingProduct.QuantityInPackage = product.QuantityInPackage;
            existingProduct.UnitOfMeasurement = product.UnitOfMeasurement;

            existingProduct.Category = newCategory;
            existingProduct.CategoryId = newCategory.Id;

            try
            {
                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Product>(existingProduct);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Product>(errorMessage);
            }
        }

        public async Task<ItemResponse<Product>> DeleteAsync(int id)
        {
            var productToDelete = await _dbContext.Products.FindAsync(id);
            if (productToDelete is null)
            {
                return new ItemResponse<Product>("Can not find product");
            }

            try
            {
                _dbContext.Products.Remove(productToDelete);
                await _dbContext.SaveChangesAsync();

                return new ItemResponse<Product>(productToDelete);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the category: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new ItemResponse<Product>(errorMessage);
            }
        }
    }
}