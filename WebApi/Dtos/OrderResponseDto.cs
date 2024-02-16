using Core.Entities.Orders;

namespace WebApi.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; }
        public Address Address { get; set; }
        public string ShippingType { get; set; } = string.Empty;
        public decimal ShippingTypePrice { get; set; }
        public IReadOnlyList<OrderItemResponseDto> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
