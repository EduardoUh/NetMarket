using Core.Entities.Orders;

namespace Core.Specifications
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string email) : base(order => order.CustomerEmail == email)
        {
            AddInclude(order => order.Items);
            AddInclude(order => order.ShippingType);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecification(int id, string email) : base(order => order.Id == id && order.CustomerEmail == email)
        {
            AddInclude(order => order.Items);
            AddInclude(order => order.ShippingType);
            AddOrderByDescending(order => order.OrderDate);
        }
    }
}
