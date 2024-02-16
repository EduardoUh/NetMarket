using Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLogic.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // stating that Address has an owner(Order), this means that it can not be created if the owner
            // hasn't been created
            //              father representation   son representation
            builder.OwnsOne(order => order.Address, address => address.WithOwner());

            // status is a Enum so we must configure it to be converted to a string when inserting it
            // to the db
            builder.Property(s => s.Status)
                .HasConversion(
                    order => order.ToString(),
                    order => (OrderStatus)Enum.Parse(typeof(OrderStatus), order)
                );
            // when the order is deleted then the items are deleted as well
            builder.HasMany(order => order.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(order => order.Subtotal)
                .HasColumnType("decimal(18,2)");
        }
    }
}
