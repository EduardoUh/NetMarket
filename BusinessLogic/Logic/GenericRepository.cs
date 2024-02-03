using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Logic
{
    public class GenericRepository<T>(NetMarketDbContext context) : IGenericRepository<T> where T : BaseClass
    {
        private readonly NetMarketDbContext _context = context;

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // the Set method create a DbSet<TEntity> than can be used to query and save instances of T
            // this means that this method can work with many entities, e.g. Product, Brand and Category
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
