using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
   public class CategoryController(IGenericRepository<Category> categoryRepository) : BaseApiController
    {
        private readonly IGenericRepository<Category> _categoryRepository = categoryRepository;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            return Ok(await _categoryRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

    }
}
