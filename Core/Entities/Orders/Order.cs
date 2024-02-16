namespace Core.Entities.Orders
{
    public class Order : BaseClass
    {
        public Order() { }
        public Order(string customerEmail, Address address, ShippingType shippingType, IReadOnlyList<OrderItem> items, decimal subtotal)
        {
            CustomerEmail = customerEmail;
            Address = address;
            ShippingType = shippingType;
            Items = items;
            Subtotal = subtotal;
        }

        public string CustomerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address Address { get; set; }
        public ShippingType ShippingType { get; set; }
        public IReadOnlyList<OrderItem> Items { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PayAttempId { get; set; }
        public decimal GetTotal()
        {
            return Subtotal + ShippingType.Price;
        }
    }
}
