using Core.Entities;
using Core.Entities.Orders;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BusinessLogic.Data
{
    public class NetMarketDbContextData
    {
        public static async Task LoadDataAsync(NetMarketDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Brand.Any())
                {
                    var brandData = File.ReadAllText(@"D:\projects\Learning\DotNet\NetMarket\NetMarket\BusinessLogic\LoadData\brand.json");
                    var brandsCollection = JsonSerializer.Deserialize<List<Brand>>(brandData);

                    await context.Brand.AddRangeAsync(brandsCollection!);

                    await context.SaveChangesAsync();
                }

                if (!context.Category.Any())
                {
                    var categoryData = File.ReadAllText(@"D:\projects\Learning\DotNet\NetMarket\NetMarket\BusinessLogic\LoadData\category.json");
                    var categoriesCollection = JsonSerializer.Deserialize<List<Category>>(categoryData);

                    await context.Category.AddRangeAsync(categoriesCollection!);
                    await context.SaveChangesAsync();
                }

                if (!context.Product.Any())
                {
                    var productData = File.ReadAllText(@"D:\projects\Learning\DotNet\NetMarket\NetMarket\BusinessLogic\LoadData\product.json");
                    var productsCollection = JsonSerializer.Deserialize<List<Product>>(productData);

                    await context.Product.AddRangeAsync(productsCollection!);
                    await context.SaveChangesAsync();
                }

                if (!context.ShippingType.Any())
                {
                    var shippingTypesData = File.ReadAllText(@"D:\projects\Learning\DotNet\NetMarket\NetMarket\BusinessLogic\LoadData\shippingType.json");
                    var shippingTypesCollection = JsonSerializer.Deserialize<List<ShippingType>>(shippingTypesData);

                    await context.ShippingType.AddRangeAsync(shippingTypesCollection!);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<NetMarketDbContextData>();
                logger.LogError(ex.Message);
            }

        }
    }
}
