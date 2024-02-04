using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class ProductController(IGenericRepository<Product> productRepository, IMapper mapper) : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        // base url = http://localhost:5158
        // api route = /api/Product
        // endpoint = /Product
        // http://localhost:5158/api/Product
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var spec = new ProductWithCategoryAndBrandSpecification();

            var products = await _productRepository.GetAllWithSpecAsync(spec);

            // mapping the list of Product objects to a list of ProductDto objects
            // when we return more than one record the result must be wrapped by the Ok method.
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
        }

        // base url = http://localhost:5158
        // api route = /api/Product
        // endpoint = /3
        // http://localhost:5158/api/Product/3
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            // spec object should include the logicall condition of the query, also the relations between the
            // entities. Product, brand and category in this case
            var spec = new ProductWithCategoryAndBrandSpecification(id);

            var product = await _productRepository.GetByIdWithSpecAsync(spec);

            if (product == null)
            {
                // default message
                //return NotFound(new CodeErrorResponse(404));
                // customized message
                return NotFound(new CodeErrorResponse(404, "The product doesn't exist"));
            }

            // mapping the Product object to a ProductDto object
            // since it is just one record it is not neccessary to wrapp it with the Ok method
            return _mapper.Map<Product, ProductDto>(product);
        }

    }
}
