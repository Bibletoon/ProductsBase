using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProductsBase.Data.Models;
using ProductsBase.Data.Repositories;
using ProductsBase.Data.Repositories.Interfaces;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CategoryService(ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger<CategoryService>();
        }

        public async Task<IEnumerable<Category>> ListAllAsync() => await _categoryRepository.ListAllAsync();

        public async Task<Page<Category>> ListAllPagedAsync(int pageNumber, int pageSize) =>
            await _categoryRepository.ListAllPagedAsync(pageNumber, pageSize);

        public async Task<ItemResponse<Category>> SaveAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                await _unitOfWork.CompleteAsync();

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
            var existingCategory = await _categoryRepository.FindByIdAsync(id);

            if (existingCategory is null)
            {
                return new ItemResponse<Category>("Category was not found");
            }

            existingCategory.Name = category.Name;

            try
            {
                _categoryRepository.Update(existingCategory);
                await _unitOfWork.CompleteAsync();

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
            var category = await _categoryRepository.FindByIdAsync(id);

            if (category is null)
            {
                return new ItemResponse<Category>("Can not find category");
            }


            try
            {
                _categoryRepository.Remove(category);
                await _unitOfWork.CompleteAsync();

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