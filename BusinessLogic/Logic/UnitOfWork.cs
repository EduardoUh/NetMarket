using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using System.Collections;

namespace BusinessLogic.Logic
{
    // the goal of the unit of work is to manage all of the repositories (Product, Brand, Order, etc.)
    public class UnitOfWork(NetMarketDbContext context) : IUnitOfWork
    {
        private readonly NetMarketDbContext _context = context;
        private Hashtable _repositories;

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // We want that when the UnitOfWork instance is deleted the context is deleted as well
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseClass
        {
            // Hastable represents a collection of key/value pairs that are organized based on the hash
            // code of the key
            // if the Hastable is null then initialize it as an empty HasTable
            _repositories ??= new Hashtable();

            // figuring out which entity(Product, Order, etc.) the TEntity is
            var type = typeof(TEntity).Name;

            // if entity is not in the Hastable then create a generic repository instance of it and add it
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                // creating the generic repository instance
                // we use the MakeGenericType because GenericRepository works with generic >_<
                // and it receives a instance of the context on its constructor, thats why we pass it as a second argument
                // CreateInstance method creates an instance of the specified type using the constructor that best matches the
                // specified params.
                // so the repositoryInstance will end up being something like this GenericRepository<Product>, GenericRepository<Order>, GenericRepository<Category>, etc.
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                // Adding the instance to the HasTable
                _repositories.Add(type, repositoryInstance);
            }

            // we must specify the type that we will be returning because we work with generics which makes the auto cast hard
            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

    }
}
