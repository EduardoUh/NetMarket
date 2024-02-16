using Core.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces
{
    public interface IGenericSecurityRepository<T> where T : IdentityUser
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
    }
}
