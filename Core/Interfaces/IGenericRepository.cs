using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseClass
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        // we use void because int this methods because unit of work will be closing the trasactions to the db
        // not this methods
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteAsync(T entity);
    }
}
