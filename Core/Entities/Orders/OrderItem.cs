namespace Core.Entities.Orders
{
    public class OrderItem : BaseClass
    {
        public OrderItem() { }
        public OrderItem(ProductOrderItem orderItem, decimal price, int amount)
        {
            OrderedItem = orderItem;
            Price = price;
            Amount = amount;
        }

        public ProductOrderItem OrderedItem { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
