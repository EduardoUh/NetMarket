using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithCategoryAndBrandSpecification : BaseSpecification<Product>
    {
        public ProductWithCategoryAndBrandSpecification()
        {
            // AddInclude is a BaseSpecification method used to add expressions to the expressions list to
            // be applied to the query (IQueryable object)
            AddInclude(p => p.Category!);
            AddInclude(p => p.Brand!);
        }

        public ProductWithCategoryAndBrandSpecification(int id) : base(p => p.Id == id)
        {
            // AddInclude is a BaseSpecification method used to add expressions to the expressions list to
            // be applied to the query (IQueryable object)
            AddInclude(p => p.Category!);
            AddInclude(p => p.Brand!);
        }
    }
}
