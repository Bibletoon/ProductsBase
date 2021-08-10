using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Data.Repositories;
using ProductsBase.Data.Repositories.Interfaces;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ProductsBase.Data.Models;

namespace ProductsBase.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public ProductService(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger<ProductService>();
        }


        public async Task<IEnumerable<Product>> ListAllAsync() => await _productRepository.ListAllAsync();

        public async Task<Page<Product>> ListAllPagedAsync(int pageNumber, int pageSize) =>
            await _productRepository.ListAllPagedAsync(pageNumber, pageSize);

        public async Task<ItemListResponse<Product>> ListByCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.FindByIdAsync(categoryId);
            if (category is null)
            {
                return new ItemListResponse<Product>("Invalid category");
            }

            var products = await _productRepository.ListByCategoryAsync(category);
            return new ItemListResponse<Product>(products);
        }

        public async Task<ItemResponse<Page<Product>>> ListByCategoryPagedAsync(int categoryId,
            int pageNumber,
            int pageSize)
        {
            var category = await _categoryRepository.FindByIdAsync(categoryId);
            if (category is null)
            {
                return new ItemResponse<Page<Product>>("Invalid category");
            }

            var products = await _productRepository.ListByCategoryPagedAsync(category, pageNumber, pageSize);
            return new ItemResponse<Page<Product>>(products);
        }

        public async Task<ItemResponse<Product>> SaveAsync(Product product)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(product.CategoryId);
                if (category is null)
                {
                    return new ItemResponse<Product>("Invalid category");
                }

                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();

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
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct is null)
            {
                return new ItemResponse<Product>("Product was not found");
            }

            var newCategory = await _categoryRepository.FindByIdAsync(product.CategoryId);

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
                _productRepository.Update(existingProduct);
                await _unitOfWork.CompleteAsync();

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
            var productToDelete = await _productRepository.FindByIdAsync(id);
            if (productToDelete is null)
            {
                return new ItemResponse<Product>("Can not find product");
            }

            try
            {
                _productRepository.Remove(productToDelete);
                await _unitOfWork.CompleteAsync();

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