using Ardalis.Specification;
using Squares.Application.Common.Models;
using Squares.Application.Common.Specification;
using Squares.Application.Identity.Users.Requests;

namespace Squares.Infrastructure.Identity.Users;

public class SearchUserSpec : SearchSpec<ApplicationUser>
{
    public SearchUserSpec(SearchUserRequest request)
        : base(request)
    {
        string? fullName = request.GetFilter<string?>("@fullName");

        Query.Where(
                x => (x.User.LastName + x.User.FirstName).Contains(fullName!) ||
                (x.User.FirstName + x.User.LastName).Contains(fullName!),
                fullName != null)
            .Include(x => x.User)
            .OrderBy(x => x.User.LastName + x.User.FirstName, !request.HasOrderBy())
            .AsNoTracking();
    }
}