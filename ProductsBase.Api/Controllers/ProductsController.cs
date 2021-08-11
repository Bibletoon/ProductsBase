using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Services.Interfaces;
using ProductsBase.Data.Models;

namespace ProductsBase.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageResource<ProductResource>),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> GetAsync([FromQuery] PageQueryParametets queryParametets,
            [FromQuery] int? categoryId)
        {
            Page<Product> products;
            if (!categoryId.HasValue)
            {
                products = await _productService.ListAllPagedAsync(queryParametets.page, queryParametets.size);
            }
            else
            {
                var result =
                    await _productService.ListByCategoryPagedAsync(categoryId.Value, queryParametets.page,
                                                                   queryParametets.size);
                if (!result.Success)
                {
                    return BadRequest(new ErrorResource(result.Message));
                }

                products = result.Item;
            }

            PageResource<ProductResource> resources =
                _mapper.Map<Page<Product>, PageResource<ProductResource>>(products);
            return Ok(resources);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PageResource<ProductResource>),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveProductResource saveProductResource)
        {
            var product = _mapper.Map<SaveProductResource, Product>(saveProductResource);

            var result = await _productService.SaveAsync(product);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            ProductResource productResource = _mapper.Map<Product, ProductResource>(result.Item);

            return Ok(productResource);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PageResource<ProductResource>),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProductResource productResource)
        {
            var product = _mapper.Map<SaveProductResource, Product>(productResource);

            var result = await _productService.UpdateAsync(id, product);
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            ProductResource updatedProductResource = _mapper.Map<Product, ProductResource>(result.Item);

            return Ok(updatedProductResource);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PageResource<ProductResource>),200)]
        [ProducesResponseType(typeof(ErrorResource),400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            ProductResource productResource = _mapper.Map<Product, ProductResource>(result.Item);
            return Ok(productResource);
        }
    }
}