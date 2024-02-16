namespace Core.Entities.Orders
{
    public class ProductOrderItem
    {
        public ProductOrderItem() { }
        public ProductOrderItem(int itemId, string itemName, string imageUrl)
        {
            ItemId = itemId;
            ItemName = itemName;
            ImageUrl = imageUrl;
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
