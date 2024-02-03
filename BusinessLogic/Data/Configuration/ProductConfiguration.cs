using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessLogic.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // here it is where we gonna add every requirement our Product table columns may have
            builder.Property(product => product.Name).IsRequired().HasMaxLength(150);
            builder.Property(product => product.Description).IsRequired().HasMaxLength(500);
            builder.Property(product => product.Image).HasMaxLength(1000);
            builder.Property(product => product.Price).IsRequired().HasColumnType("decimal(18,2)");
            // stating that the product has one brand and the foreign key for that brand will be BrandId
            // the WithMany method configures the relationship as one-to-many which means
            // that one brand can have many products
            builder.HasOne(brand => brand.Brand).WithMany().HasForeignKey(product => product.BrandId);
            // same as above but with category
            builder.HasOne(category => category.Category).WithMany().HasForeignKey(product => product.CategoryId);
        }
    }
}
