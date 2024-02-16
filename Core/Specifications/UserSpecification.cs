using Core.Entities;

namespace Core.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(UserSpecificationParams userParams)
            : base(user =>
                    (string.IsNullOrEmpty(userParams.Search) || user.Name.Contains(userParams.Search)) &&
                    (string.IsNullOrEmpty(userParams.Name) || user.Name.Contains(userParams.Name)) &&
                    (string.IsNullOrEmpty(userParams.LastName) || user.LastName.Contains(userParams.LastName))
                  )
        {
            ApplyPagination(userParams.PageSize, userParams.PageSize * (userParams.PageIndex - 1));

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(u => u.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(u => u.Name);
                        break;
                    case "emailAsc":
                        AddOrderBy(u => u.Email!);
                        break;
                    case "emailDesc":
                        AddOrderByDescending(u => u.Email!);
                        break;
                    case "lastnameAsc":
                        AddOrderBy(u => u.LastName);
                        break;
                    case "lastnameDesc":
                        AddOrderByDescending(u => u.LastName);
                        break;
                    default:
                        AddOrderBy(u => u.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(u => u.Name);
            }
        }
    }
}
