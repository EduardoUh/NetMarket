using Core.Entities.Orders;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(string customerEmail, int shippingType, string shoppingCartId, Address address);
        Task<IReadOnlyList<Order>> GetOrderByUserEmailAsync(string email);
        Task<Order> GetOrderByIdAsync(int id, string email);
        Task<IReadOnlyList<ShippingType>> GetShippingTypesAsync();
    }
}
