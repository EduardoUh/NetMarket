namespace Core.Entities.Orders
{
    public class ShippingType : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
