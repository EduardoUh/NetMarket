using Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLogic.Data.Configuration
{
    public class OrderITemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // declaring the 1:1 relationship between OrderItem(father) and ProductOrderItem(son)
            builder.OwnsOne(orderItem => orderItem.OrderedItem, orderedItem => { orderedItem.WithOwner(); });

            //configuring the price
            builder.Property(orderItem => orderItem.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
