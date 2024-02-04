using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BrandController(IGenericRepository<Brand> brandRepository) : BaseApiController
    {
        private readonly IGenericRepository<Brand> _brandRepository = brandRepository;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await _brandRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

    }
}
