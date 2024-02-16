using Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLogic.Data.Configuration
{
    public class ShippingTypeConfiguration : IEntityTypeConfiguration<ShippingType>
    {
        public void Configure(EntityTypeBuilder<ShippingType> builder)
        {
            builder.Property(shippingType => shippingType.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
