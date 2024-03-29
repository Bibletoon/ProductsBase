﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageResource<CategoryResource>),200)]
        public async Task<PageResource<CategoryResource>> GetAllAsync([FromQuery] PageQueryParametets queryParametets)
        {
            var categories = await _categoryService.ListAllPagedAsync(queryParametets.page, queryParametets.size);
            PageResource<CategoryResource>
                map = _mapper.Map<Page<Category>, PageResource<CategoryResource>>(categories);
            return map;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResource),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> PostCategoryAsync([FromBody] SaveCategoryResource saveCategoryResource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(saveCategoryResource);

            var result = await _categoryService.SaveAsync(category);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            CategoryResource categoryResource = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryResource),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> PutCategoryAsync(int id,
            [FromBody] SaveCategoryResource updateCategoryResource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(updateCategoryResource);

            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            CategoryResource categoryResource = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategoryResource),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            CategoryResource categoryResourse = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResourse);
        }
    }
}