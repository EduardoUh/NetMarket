using Core.Entities;

namespace Core.Interfaces
{
    // IDisposable provides a mechanism for releasing unmanaged resources, which means that after we
    // use the unit of work class whe can release it
    public interface IUnitOfWork : IDisposable
    {
        // this method returns the instance of a repository, based in the entity we set
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseClass;

        Task<int> Complete();
    }
}
