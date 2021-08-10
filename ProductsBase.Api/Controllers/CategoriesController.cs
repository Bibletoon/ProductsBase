using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Services.Interfaces;
using ProductsBase.Data.Models;

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
        public async Task<PageResource<CategoryResource>> GetAllAsync([FromQuery] PageQueryParametets queryParametets)
        {
            var categories = await _categoryService.ListAllPagedAsync(queryParametets.page, queryParametets.size);
            PageResource<CategoryResource>
                map = _mapper.Map<Page<Category>, PageResource<CategoryResource>>(categories);
            return map;
        }

        [HttpPost]
        public async Task<IActionResult> PostCategoryAsync([FromBody] SaveCategoryResource saveCategoryResource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(saveCategoryResource);

            var result = await _categoryService.SaveAsync(category);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            CategoryResource categoryResource = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryAsync(int id,
            [FromBody] SaveCategoryResource updateCategoryResource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(updateCategoryResource);

            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            CategoryResource categoryResource = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            CategoryResource categoryResourse = _mapper.Map<Category, CategoryResource>(result.Item);

            return Ok(categoryResourse);
        }
    }
}