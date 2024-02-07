using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        // helper method that is used to apply the criteria, includes, pagination, etc. to the IQueryable object and return it ready
        // to be executed
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            // attaching entity to the context
            _context.Set<T>().Attach(entity);
            // stating that the entity state is modified
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }
    }
}
