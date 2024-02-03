using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        // This method will return a collection of products, so we can use List<Product>, IEnumerable<Product>
        // and the IReadonlyList<Product>, in this case we don't wanna modify the list so we gonna use the last one

    }
}
