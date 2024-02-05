using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // logicall condition you want to be applied to an entity
        Expression<Func<T, bool>> Criteria { get; }

        // entities to be included in the query
        List<Expression<Func<T, object>>> Includes { get; }

        // this is taken as ascending by default
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        // pagination properties
        int Take { get; }
        int Skip { get; }
        bool IsPaginationEnabled { get; }
    }
}
