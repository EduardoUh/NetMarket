using Core.Entities;

namespace Core.Specifications
{
    public class ProductForCountingSpecification(ProductSpecificationParams productParams)
        : BaseSpecification<Product>(p =>
                       (string.IsNullOrEmpty(productParams.Search) || p.Name.Contains(productParams.Search)) &&
                       (!productParams.Brand.HasValue || p.BrandId == productParams.Brand) &&
                       (!productParams.Category.HasValue || p.CategoryId == productParams.Category)
            )
    {
    }
}
