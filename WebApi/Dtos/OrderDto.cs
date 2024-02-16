namespace WebApi.Dtos
{
    public class OrderDto
    {
        public string ShoppingCartId { get; set; } = string.Empty;
        public int ShippingType { get; set; }
        public required AddressDto Address { get; set; }
    }
}
