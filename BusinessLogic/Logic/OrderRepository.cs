using Core.Entities;
using Core.Entities.Orders;
using Core.Interfaces;
using Core.Specifications;

namespace BusinessLogic.Logic
{
    public class OrderRepository(
        // IGenericRepository<Order> orderRepository,
        // IGenericRepository<Product> productRepository,
        // IGenericRepository<ShippingType> shippingTypeRepository,
        // unit of work is replacing all of the repositories above
        IUnitOfWork unitOfWork,
        IShoppingCartRepository shoppingCartRepository
        ) : IOrderRepository
    {
        // private readonly IGenericRepository<Order> _orderRepository = orderRepository;
        // private readonly IGenericRepository<Product> _productRepository = productRepository;
        // private readonly IGenericRepository<ShippingType> _shippingTypeRepository = shippingTypeRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IShoppingCartRepository _shoppingCartRepository = shoppingCartRepository;

        public async Task<Order> AddOrderAsync(string customerEmail, int shippingType, string shoppingCartId, Core.Entities.Orders.Address address)
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync(shoppingCartId);

            var items = new List<OrderItem>();

            foreach (var item in shoppingCart.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var orderedItem = new ProductOrderItem(productItem.Id, productItem.Name, productItem.Image!);

                var orderITem = new OrderItem(orderedItem, productItem.Price, item.Amount);

                items.Add(orderITem);
            }

            var shippingTypeEntity = await _unitOfWork.Repository<ShippingType>().GetByIdAsync(shippingType);

            var subTotal = items.Sum(item => item.Price * item.Amount);

            var order = new Order
            {
                CustomerEmail = customerEmail,
                Address = address,
                ShippingType = shippingTypeEntity,
                Items = items,
                Subtotal = subTotal
            };

            // TODO: save the order to the db
            _unitOfWork.Repository<Order>().AddEntity(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            await _shoppingCartRepository.DeleteShoppingCartAsync(shoppingCartId);

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string email)
        {
            var spec = new OrderWithItemsSpecification(id, email);

            return await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrderByUserEmailAsync(string email)
        {
            var spec = new OrderWithItemsSpecification(email);

            return await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<ShippingType>> GetShippingTypesAsync()
        {
            return await _unitOfWork.Repository<ShippingType>().GetAllAsync();
        }
    }
}
