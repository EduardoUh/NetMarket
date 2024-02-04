using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IGenericRepository<Product> productRepository) : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository = productRepository;

        // base url = http://localhost:5158
        // api route = /api/Product
        // endpoint = /Product
        // http://localhost:5158/api/Product
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var spec = new ProductWithCategoryAndBrandSpecification();
            // when we return more than one record the result must be wrapped by the Ok method.
            return Ok(await _productRepository.GetAllWithSpecAsync(spec));
        }

        // base url = http://localhost:5158
        // api route = /api/Product
        // endpoint = /3
        // http://localhost:5158/api/Product/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            // spec object should include the logicall condition of the query, also the relations between the
            // entities. Product, brand and category in this case
            var spec = new ProductWithCategoryAndBrandSpecification(id);

            // since it is just one record it is not neccessary to wrapp it with the Ok method
            return await _productRepository.GetByIdWithSpecAsync(spec);
        }

    }
}
