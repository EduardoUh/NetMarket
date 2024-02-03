using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Logic
{
    public class ProductRepository(NetMarketDbContext context) : IProductRepository
    {
        private readonly NetMarketDbContext _context = context;

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _context.Product
                .Include(product => product.Brand)
                .Include(product => product.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Product
                .Include(product => product.Brand)
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id);
        }
    }
}
