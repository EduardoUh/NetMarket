using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithCategoryAndBrandSpecification : BaseSpecification<Product>
    {
        public ProductWithCategoryAndBrandSpecification(ProductSpecificationParams productParams)
            : base(p =>
                       (string.IsNullOrEmpty(productParams.Search) || p.Name.Contains(productParams.Search)) &&
                       (!productParams.Brand.HasValue || p.BrandId == productParams.Brand) &&
                       (!productParams.Category.HasValue || p.CategoryId == productParams.Category)
            )
        {
            // AddInclude is a BaseSpecification method used to add expressions to the expressions list to
            // be applied to the query (IQueryable object)
            AddInclude(p => p.Category!);
            AddInclude(p => p.Brand!);
            // hard coded sorting
            // AddOrderBy(p => p.Name);
            // AddOrderByDescending(p => p.Name);

            // dynamic sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "NameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "NameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    case "DescriptionAsc":
                        AddOrderBy(p => p.Description);
                        break;
                    case "DescriptionDesc":
                        AddOrderByDescending(p => p.Description);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }

            // pagination
            ApplyPagination(productParams.PageSize, productParams.PageSize * (productParams.PageIndex - 1));
        }

        public ProductWithCategoryAndBrandSpecification(int id) : base(p => p.Id == id)
        {
            // AddInclude is a BaseSpecification method used to add expressions to the expressions list to
            // be applied to the query (IQueryable object)
            AddInclude(p => p.Category!);
            AddInclude(p => p.Brand!);
            AddOrderBy(p => p.Name);
            // AddOrderByDescending(p => p.Name);
        }
    }
}
